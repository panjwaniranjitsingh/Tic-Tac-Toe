using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.EssentialKit;

public class MailService : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private Transform contactPrefab;
    
    private void OnEnable()
    {
        StartCoroutine(Show());
    }

    private IEnumerator Show()
    {
        yield return new WaitForSeconds(1f);
        AddressBookService.instance.ReadContacts();
        yield return new WaitForSeconds(1f);
        ShowContacts();
    }
    private void ShowContacts()
    {
        IAddressBookContact[] contacts = AddressBookService.instance.allContacts;
        if (contacts.Length > 0)
        {
            foreach (IAddressBookContact contact in contacts)
            {
                CreateContactUI(contact);
            }
        }
        else
        {
            CreateContactUI(null);
        }
    }

    private void CreateContactUI(IAddressBookContact contact)
    {
        Transform contactGO = Instantiate(contactPrefab,content);
        Text buttonText = contactGO.GetChild(0).GetComponent<Text>();
        Button emailButton = contactGO.GetChild(1).GetComponent<Button>();
        if (contact != null)
        {
            buttonText.text = contact.FirstName + " " + contact.LastName;
            emailButton.interactable = contact.EmailAddresses.Length > 0;
            emailButton.onClick.AddListener(()=> {
                SendMail(contact);
            });
        }
        else
        {
            buttonText.text = "No contacts to display.";
            emailButton.gameObject.SetActive(false);
        }
        contactGO.gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        foreach(Transform contact in content)
        {
            Destroy(contact.gameObject);
        }
    }

    private void SendMail(IAddressBookContact contact)
    {
        bool canSendMail = MailComposer.CanSendMail();
        if (!canSendMail)
            return;
        MailComposer composer = MailComposer.CreateInstance();
        foreach (string emailID in contact.EmailAddresses)
        {
            composer.SetToRecipients(emailID);
            //composer.SetCcRecipients(new string[1] { "cc@gmail.com" });
            //composer.SetBccRecipients(new string[1] { "bcc@gmail.com" });

            composer.SetSubject("Invitation using VoxelBusters.EssentialKit");
            string bodyText = "Hi "+contact.FirstName + " " +contact.LastName+ ", <br> This is an invitation mail send using Mail Composer of Cross Platform Native Plugins: Essential Kit created by Voxel Busters.";
            composer.SetBody(bodyText, true);//Pass true if string is html content
            composer.SetCompletionCallback((result, error) =>
            {
                Debug.Log("Mail composer was closed. Result code: " + result.ResultCode);
            });
            composer.Show();
        }
    }
}
