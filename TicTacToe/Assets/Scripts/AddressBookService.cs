using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

public class AddressBookService : MonoBehaviour
{
    public static AddressBookService instance;

    private AddressBookContactsAccessStatus status;

    public IAddressBookContact[] allContacts;
    
    private void Awake()
    {
        instance = this;
    }

    public void ReadContacts()
    {
        status = AddressBook.GetContactsAccessStatus();
        if(status == AddressBookContactsAccessStatus.NotDetermined)
        {
            AddressBook.RequestContactsAccess(callback: OnRequestContactsAccessFinish);
        }
        if(status == AddressBookContactsAccessStatus.Authorized)
        {
            AddressBook.ReadContacts(OnReadContactsFinish);
        }
    }

    private void OnRequestContactsAccessFinish(AddressBookRequestContactsAccessResult result, Error error)
    {
        Debug.Log("Request for contacts access finished.");
        Debug.Log("Address book contacts access status: " + result.AccessStatus);
        if (result.AccessStatus == AddressBookContactsAccessStatus.Authorized)
        {
            AddressBook.ReadContacts(OnReadContactsFinish);
        }
    }

    private void OnReadContactsFinish(AddressBookReadContactsResult result, Error error)
    {
        if (error == null)
        {
            var contacts = result.Contacts;
            Debug.Log("Request to read contacts finished successfully.");
            Debug.Log("Total contacts fetched: " + contacts.Length);
            Debug.Log("Below are the contact details (capped to first 10 results only):");
            for (int iter = 0; iter < contacts.Length && iter < 10; iter++)
            {
                Debug.Log(string.Format("[{0}]: {1}", iter, contacts[iter]));
            }
            allContacts = result.Contacts;
        }
        else
        {
            Debug.Log("Request to read contacts failed with error. Error: " + error);
        }
    }
}
