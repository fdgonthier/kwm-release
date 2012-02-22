#include "MAPIAddressBook.h"

using namespace System::Runtime::InteropServices;
using namespace System::Collections::Generic;

// This function returns 'true' if the user has at least one profile 
// configured.
bool HasProfile()
{
    HKEY profKey = NULL;
    wchar_t key[] = L"Software\\Microsoft\\Windows NT\\CurrentVersion\\Windows Messaging Subsystem\\Profiles";
    bool hasProfile = false;
    DWORD subKeys;
    LONG r;

    r = RegOpenKeyEx(HKEY_CURRENT_USER, key, 0, KEY_READ, &profKey);
    if (r != ERROR_SUCCESS)
        return false;
    else
    {
        // This is a totally retarded function...
        r = RegQueryInfoKey(profKey, NULL, NULL, NULL, &subKeys, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
        if (subKeys >= 1)
            hasProfile = true;
    }

    if (profKey != NULL)
        RegCloseKey(profKey);

    return hasProfile;
}

// There doesn't seem to be a way to tell MAPILogonEx to show
// the profile selection dialog only when the user has explicitely
// said so in the Mail configuration. This works around that by
// checking if the option is set in the registry.
bool MustChooseProfile()
{
    HKEY pickKey = NULL;
    wchar_t key[] = L"Software\\Microsoft\\Exchange\\Client\\Options";
    LONG r;
    wchar_t regData[3]; // This key is supposed to be "1" or "2"
    DWORD szregData = sizeof(regData);
    bool mustChooseProfile;

    // If the key is missing, MAPI should assume a default profile
    r = RegOpenKeyEx(HKEY_CURRENT_USER, key, 0, KEY_READ, &pickKey);
    if (r != ERROR_SUCCESS)
        return false;
    else
    {
        // If the value is "1" then it means the user has demanded
        // to choose the profile he wants to use and the MAPI login
        // dialog should be displayed on call to Logon.

        // If the key is missing, MAPI assume a default profile.
        r = RegGetValue(pickKey, NULL, L"PickLogonProfile", RRF_RT_REG_SZ, NULL, (void *)&regData, &szregData);
        if (r != ERROR_SUCCESS)
            mustChooseProfile = false;

        if (wcsncmp(L"1", (wchar_t *)regData, szregData) == 0)
            mustChooseProfile = true;
    }

    if (pickKey != NULL)
        RegCloseKey(pickKey);

    return mustChooseProfile;
}

static void SafeRelease(IUnknown *p)
{
    if (p) 
    {
        p->Release();
        p = NULL;
    }
}

void MAPIAddressBook::DistListToSMTP(IDistList *pList, List<String^>^ emails)
{
    IMAPITable *contentTable = NULL;
    SPropTagArray ptags;
    SRowSet *prows = NULL;
    HRESULT r = S_OK;
    wchar_t openErr[] = L"Failed to access distribution list";

    try
    {
        r = pList->GetContentsTable(0, &contentTable);
        if (FAILED(r)) 
            ThrowMAPIError(r, openErr);

        ptags.aulPropTag[0] = PR_ENTRYID;
        ptags.cValues = 1;

        // Tell MAPI we just want the Entry ID.
        r = contentTable->SetColumns(&ptags, 0);
        if (FAILED(r)) 
            ThrowMAPIError(r, openErr);

        // Fetch elements of the mailing list one by one.
        while (true)
        {
            r = contentTable->QueryRows(1, 0, &prows);
            if (FAILED(r)) 
                ThrowMAPIError(r, openErr);

            // Check for the end of the table, this is okay.
            if (prows->cRows == 0) return;

            // Convert the entry IDs to email strings.
            ULONG szeid = prows->aRow[0].lpProps->Value.bin.cb;
            ENTRYID *peid = (ENTRYID *)prows->aRow[0].lpProps->Value.bin.lpb;

            EntryIDToSMTP(szeid, peid, emails);
        }
    }
    finally
    {
        if (prows != NULL)
            FreeProws(prows);

        SafeRelease(contentTable);
    }
}

String^ MAPIAddressBook::MailUserToSMTP(IMailUser *pUser)
{
    SPropTagArray ptags;
    ULONG cv;
    SPropValue *pv = NULL;
    wchar_t errMsg[] = L"Failed to obtain SMTP address from user";

    try
    {
        // Try to get the PR_SMTP_ADDRESS property.
        ptags.aulPropTag[0] = PROP_TAG(PT_UNICODE, 0x39FE);
        ptags.cValues = 1;

        HRESULT r = pUser->GetProps(&ptags, MAPI_UNICODE, &cv, &pv);

        // Return nothing if we can't get that property.
        if (FAILED(r)) 
            ThrowMAPIError(r, errMsg);
        else
            return Marshal::PtrToStringUni(IntPtr(pv->Value.lpszW));
    }
    finally
    {
        if (pv != NULL)
            MAPIFreeBuffer(pv);
    }

    // There is no way this code will be reached.
    throw gcnew MAPIException(errMsg);
}

void MAPIAddressBook::EntryIDToSMTP(size_t cbEntryID, ENTRYID *pEntryID, List<String^>^ emails)
{
    ULONG objType;
    IUnknown *pUnk = NULL;
    array<String^>^ retArray = nullptr;
    HRESULT r;

    try
    {
        // Open the address book entry corresponding to the
        // entry ID we got from the dialog.
        r = pAddressBook->OpenEntry(cbEntryID, pEntryID, NULL, 0, &objType, &pUnk);
        if (FAILED(r))
            ThrowMAPIError(r, L"Failed to open address book entry");

        // Single users.
        if (objType == MAPI_MAILUSER)
            emails->Add(MailUserToSMTP((IMailUser *)pUnk));

        // Distribution lists.
        else if (objType == MAPI_DISTLIST || objType == MAPI_ABCONT)
            DistListToSMTP((IDistList *)pUnk, emails);
    }
    finally
    {
        SafeRelease(pUnk);
    }
}

void MAPIAddressBook::AdrEntryToSMTP(ADRENTRY *adrEntry, List<String^>^ emails)
{
    int eidIdx = -1, smtpIdx = -1, emailIdx = -1;
    bool emailIsSMTP = false;
    int objectType;

    // Search for the interesting properties.
    for (size_t j = 0; j < adrEntry->cValues; j++)
    {
        SPropValue pval = adrEntry->rgPropVals[j];

        // Save the email address.
        if (pval.ulPropTag == PR_EMAIL_ADDRESS)
            emailIdx = j;

        // Check if the type of address we have is something
        // we can use directly.
        if (pval.ulPropTag == PR_ADDRTYPE)
            emailIsSMTP = (wcscmp(pval.Value.lpszW, L"SMTP") == 0);

        // Get the object type since different handling needs
        // to be done for mailing lists.
        if (pval.ulPropTag == PR_OBJECT_TYPE)
            objectType = pval.Value.l;

        // This is PR_SMTP_ADDRESS.
        if (PROP_ID(pval.ulPropTag) == 0x39FE)
            smtpIdx = j;

        if (pval.ulPropTag == PR_ENTRYID)
            eidIdx = j;

        // Break if we have already found our interesting
        // properties.
        if (eidIdx != -1 && smtpIdx != -1)
            break;
    }

    // If we have the SMTP address, use that.
    if (objectType == MAPI_MAILUSER && smtpIdx != -1)
        emails->Add(Marshal::PtrToStringUni(IntPtr(adrEntry->rgPropVals[smtpIdx].Value.lpszW)));

    // If we have a PR_EMAIL_ADDRESS of type SMTP, use that.
    else if (objectType == MAPI_MAILUSER && emailIsSMTP && emailIdx != -1)
        emails->Add(Marshal::PtrToStringUni(IntPtr(adrEntry->rgPropVals[emailIdx].Value.lpszW)));

    // If don't have the email address, we use the
    // entry ID to fetch the address data from the
    // address book.
    else if (eidIdx != -1)
    {
        size_t pszeid = adrEntry->rgPropVals[eidIdx].Value.bin.cb;
        ENTRYID *peid = (ENTRYID *)adrEntry->rgPropVals[eidIdx].Value.bin.lpb;
        EntryIDToSMTP(pszeid, peid, emails);
    }

    // Something really weird happened.
    else
        throw gcnew MAPIException(L"Invalid entry received.");
}

void MAPIAddressBook::ThrowMAPIError(HRESULT hResult, wchar_t *altText)
{
    LPMAPIERROR mapiErr;
    char buf[1024];

    // If GetLastError fails, throw the alternative text.
    if (pSession == NULL 
        || FAILED(pSession->GetLastError(hResult, MAPI_UNICODE, &mapiErr))
        || mapiErr == NULL)
    {
        wsprintf((wchar_t *)buf, L"%s (HRESULT = 0x%x)", altText, hResult);
        throw gcnew MAPIException((wchar_t *)buf);
    }
    else
    {
        // Use the MAPI-provided message.
        try
        {
            wsprintf((wchar_t *)buf, L"%s (%s)", altText, mapiErr->lpszError);
            throw gcnew MAPIException((wchar_t *)buf);
        }
        finally
        {
            MAPIFreeBuffer(mapiErr);
        }
    }
}

bool MAPIAddressBook::LoggedOn()
{
    return pSession != NULL && pAddressBook != NULL;
}

void MAPIAddressBook::Logon()
{
    char buf[1024];
    IMAPISession *pSess;
    IAddrBook *pAddrBook;
    ULONG flags;
    HRESULT r;

    // Check if we are already logged on, and logoff if it's
    // the case.
    if (pSession != NULL && pAddressBook != NULL) 
        Logoff();
        
    flags = MAPI_ALLOW_OTHERS | MAPI_EXTENDED | MAPI_LOGON_UI | MAPI_NO_MAIL | MAPI_UNICODE;

    if (!MustChooseProfile())
        flags |= MAPI_USE_DEFAULT;

    // Get the MAPI session.
    r = MAPILogonEx(NULL, NULL, NULL, flags, &pSess);
    if (FAILED(r))
    {
        wsprintf((wchar_t *)buf, L"MAPI logon failed (HRESULT = 0x%x)", r);
        ThrowMAPIError(r, (wchar_t *)buf);
    }
    else
        pSession = pSess;

    // Get the address book.
    r = pSession->OpenAddressBook(NULL, NULL, AB_NO_DIALOG, &pAddrBook);
    if (FAILED(r)) 
    {
        wsprintf((wchar_t *)buf, L"Failed to open the address book (HRESULT = 0x%x)", r);
        ThrowMAPIError(r, (wchar_t *)buf);
    }
    else
        pAddressBook = pAddrBook;
}

void MAPIAddressBook::Logoff()
{
    SafeRelease(pAddressBook);

    if (pSession != NULL) {
        pSession->Logoff(NULL, NULL, NULL);
        SafeRelease(pSession);
    }
}

MAPIAddressBook::MAPIAddressBook(void)
{
    MAPIINIT_0 pInit;

    pSession = NULL;
    pAddressBook = NULL;

    // Check if there are any profile in the registry.
    if (!HasProfile())
        throw gcnew System::Exception(L"No profile configured.");

    // Try to initialize the MAPI system.
    pInit.ulVersion = MAPI_INIT_VERSION;
    pInit.ulFlags = MAPI_MULTITHREAD_NOTIFICATIONS | 0x8 /* MAPI_NO_COINIT */;

    HRESULT r = MAPIInitialize(&pInit);
    if (FAILED(r))
    {
        char buf[1024];
        wsprintf((wchar_t *)buf, L"MAPI initializion failed (HRESULT = 0x%x)", r);
        ThrowMAPIError(r, (wchar_t *)buf);
    }
}

MAPIAddressBook::~MAPIAddressBook(void)
{
    this->!MAPIAddressBook();
}

MAPIAddressBook::!MAPIAddressBook(void)
{
    Logoff();

    // Goodbye MAPI.
    MAPIUninitialize();
}

array<String^>^ MAPIAddressBook::Show(IntPtr hwnd)
{
    ADRPARM adrParm;
    LPADRLIST adrList = NULL;
    ULONG nFields[] = {MAPI_TO};
    LPTSTR sFields[1];
    IntPtr tsCaption = IntPtr::Zero;
    IntPtr tsFieldCaption = IntPtr::Zero;
    IntPtr tsFieldGroupCaption = IntPtr::Zero;
    IntPtr tsNewEntryCaption = IntPtr::Zero;
    ULONG_PTR pHwnd = (ULONG_PTR)hwnd.ToPointer();
    HRESULT r;

    if (!LoggedOn())
        throw gcnew MAPIException(L"Not logged on.");

    // This code display Outlook address book dialog.
    try
    {
        // Move the string to unmanaged space.
        if (windowCaption != nullptr)
            tsCaption = Marshal::StringToHGlobalUni(windowCaption);
        if (fieldCaption != nullptr)
            tsFieldCaption = Marshal::StringToHGlobalUni(fieldCaption);
        if (newEntryCaption != nullptr)
            tsNewEntryCaption = Marshal::StringToHGlobalUni(newEntryCaption);
        if (fieldGroupCaption != nullptr)
            tsFieldGroupCaption = Marshal::StringToHGlobalUni(fieldGroupCaption);
        if (fieldCaption != nullptr)
            sFields[0] = (LPTSTR)tsFieldCaption.ToPointer();

        memset(&adrParm, 0, sizeof(ADRPARM));
        adrParm.lpszCaption = (LPTSTR)tsCaption.ToPointer();
        adrParm.lpszNewEntryTitle = (LPTSTR)tsNewEntryCaption.ToPointer();
        adrParm.lpszDestWellsTitle = (LPTSTR)tsFieldGroupCaption.ToPointer();
        adrParm.cDestFields = 1;

        // Leave the field caption alone if the caller has not
        // set any.
        if (fieldCaption != nullptr) 
        {
            adrParm.lpulDestComps = nFields;
            adrParm.lppszDestTitles = sFields;
        }

        adrParm.ulFlags = DIALOG_MODAL | AB_RESOLVE | 0x40 /* AB_UNICODEUI */ | MAPI_UNICODE;

        // If selectOne is true, prevent the use from selecting
        // multiple recipients.
        if (selectOne)
            adrParm.ulFlags |= ADDRESS_ONE;

        r = pAddressBook->Address(&pHwnd, &adrParm, &adrList);

        // Trap plain-old cancellation.
        if (FAILED(r) && r == MAPI_E_USER_CANCEL) 
            return nullptr;
        else if (FAILED(r))
            ThrowMAPIError(r, L"Dialog error");
    }
    finally
    {
        Marshal::FreeHGlobal(tsCaption);
        Marshal::FreeHGlobal(tsFieldCaption);
    }

    // At this point, we must have a valid ADRLIST.
    try
    {
        List<String^>^ retList = gcnew List<String^>();

        // Iterate through all the entries and get the SMTP address 
        // in the user properties.
        for (size_t i = 0; i < adrList->cEntries; i++)
        {
            ADRENTRY adrEntry = adrList->aEntries[i];
            AdrEntryToSMTP(&adrEntry, retList);
        }

        return retList->ToArray();            
    }
    finally
    {
        // Free the ADRLIST.
        FreePadrlist(adrList);
    }
}