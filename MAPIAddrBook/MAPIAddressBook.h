#pragma once

#define INITGUID
#define USES_IID_IMailUser
#define USES_IID_IMAPIProp

#include <windows.h>
#include <mapix.h>
#include <mapiutil.h>
#include <mapiguid.h>

using namespace System;
using namespace System::Collections::Generic;

public ref class MAPIException : System::Exception
{
public:
    MAPIException(wchar_t *errMsg)
        : System::Exception(gcnew String(errMsg))
    {
    }
};

public ref class MAPIAddressBook
{
private:
    IMAPISession *pSession;
    IAddrBook *pAddressBook;

    void EntryIDToSMTP(size_t cbEntryID, ENTRYID *pEntryID, List<String^>^ emails);

    void AdrEntryToSMTP(ADRENTRY *adrEntry, List<String^>^ emails);

    void DistListToSMTP(IDistList *pList, List<String^>^ emails);

    String^ MailUserToSMTP(IMailUser *pUser);

    void ThrowMAPIError(HRESULT hResult, wchar_t *altText);

    // SelectOne property.
    bool selectOne;

    // WindowCaption property.
    String^ windowCaption;

    // FieldCaption property.
    String^ fieldCaption;

    // NewEntryCaption property.
    String^ newEntryCaption;

    // FieldGroupCaption property.
    String^ fieldGroupCaption;

    // Logon error property.
    String^ logonError;

public:
    /**
    Open the MAPI system. This will throw an exception if this
    component is not usable.
    */
    MAPIAddressBook(void);

    ~MAPIAddressBook();

    !MAPIAddressBook();

    /**
    This attempts logon to the MAPI system. Throws an exception if
    the user cancels a logon dialog, if we can't login to MAPI or
    if we can't open the address book. This will call the Logoff
    method if there has been a login done already so it's preferable
    to make sure you don't call this more than you need. Use LoggedOn
    to see if you need to call logon.
    */
    void Logon();

    /**
    This logoff the MAPI system. Call this method when you no longer
    need to use the dialog. You can call this method as many time as
    you want safely.
    */
    void Logoff();

    /**
    This returns true if component is logged on to MAPI.
    */
    bool LoggedOn();

    /**
    This is the main function that will show the dialog
    box to the user. This returns a list of SMTP email address
    from whatever address book is displayed. This returns NULL
    when the dialog is cancelled.
    */
    array<String^>^ Show(IntPtr hwnd);

    /**
    The window title of the dialog.
    */
    property String^ WindowCaption
    {
        String^ get()
        {
            return windowCaption;
        }

        void set(String^ value)
        {
            windowCaption = value;
        }
    }

    /**
    This is the field caption. This is displayed at beside the
    text box at the bottom of the dialog window. The dialog itself
    supports multiple field like this but only one is supported.
    */
    property String^ FieldCaption
    {
        String^ get()
        {
            return fieldCaption;
        }

        void set(String^ value)
        {
            fieldCaption = value;
        }
    }

    /**
    Set this if you want the dialog to allow a single
    selection only.
    */
    property bool SelectOne
    {
        bool get()
        {
            return selectOne;
        }
        
        void set(bool value)
        {
            selectOne = value;
        }
    }

    /**
    Caption of the text displayed in the 'New Entry' dialog which 
    can be invoked by the user from the dialog box.
    */
    property String^ NewEntryCaption
    {
        String^ get()
        {
            return newEntryCaption;
        }

        void set(String^ value)
        {
            newEntryCaption = value;
        }
    }

    /**
    Caption of the group of field displayed below the main content of
    the dialog, but just above the field themselves.
    */
    property String^ FieldGroupCaption
    {
        String^ get()
        {
            return fieldGroupCaption;
        }

        void set(String^ value)
        {
            fieldGroupCaption = value;
        }
    }
};
