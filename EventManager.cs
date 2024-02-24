using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    #region Singleton
    //Singleton stuff
    public static EventManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region Explanation
    /* So how does this work?
     * There are 3 main participants:
     *  Author - The script that triggers the event
     *  Publisher - The middleman, this script who carries the information
     *  Subscriber - The script(s) who listen to the events and possibly get triggered
     * Below you will find 2 examples. One event that doesn't carry informations and one that does.
     * 
     * Here's how *our* events are built (as there are many other ways to do it. After 7 hours of errors, this is the best way to it for us.

    * Event is composed of 3 parts:
    *   Delegate - carries information
    *   Event - the event itself that is a type of the delegate
    *   Trigger (it's a method) - triggers the event, if there are subscribers to it.
    *   
    *   1. You trigger the event other script.
    *   2. The publisher registers it and informs all the subscribers.
    *   3. Subscribers then trigger their own methods that are dependent on that event.
    */

    //Event with no information

    // Delegate that returns nothing with no arguments. Confusingly, this is the name of the event. I know.
    public delegate void NoInformation();

    //Events usually have the 'On' prefix + the delegate.
    public static event NoInformation OnNoInformation;

    //We must check if there are subscribers, otherwise it is null.
    //Not really a naming convention for this. I think we should do TR_delegate -> TR_OnNoInformation()
    //The above is shorter way to check if its null when you don't need anything else in it but wrote the long version just in case it's not clear.
    public void TriggerNoInformation() => OnNoInformation?.Invoke();
    //public void TriggerNoInformation()
    //{
    //    if (OnNoInformation != null) //->only null if no subs
    //    {
    //        OnNoInformation();
    //    }
    //}

    //So how does this look? I will use functions you are familiar with to make it clear.

    //This is ChooseArmor.cs
    //public void NextOption()
    //{
    //This function's own stuff.
    //..
    //..
    //Then you use the trigger function like any other function to trigger.
    //EventManager.instance.TriggerNoInformation();
    //}
    
    //Event is triggered and the EventManager.cs sees it, informs subscribers
    
    //How to subscribe:
    //PrinceStats.cs - in this case this is the subcsriber, listening for the trigger. Remember, there can be multiple subscribers to the same event.
    //You subsribe by typing down the event += the function that gets triggered when the event is triggered.
    //You unsubsubcribe with -=. You should do that to prevent memory leak.
    // If you put = only, that function will be the sole subcriber to that event preventing other subscribers.
    //
    //void OnEnable()
    //{
    //  !!!!!!! NO () AFTER THE FUNCTION
    //  EventManager.OnNoInformation += AddArmor; 
    //}

    //void OnDisable()
    //{
    //  !!!!!!! NO () AFTER THE FUNCTION
    //  EventManager.OnNoInformation -= AddArmor;
    //}

    /*What happens:
     * When player presses Next Option button, the event gets triggered.
     * EventManager registers that the Button has been pressed.
     * EventManagers informs every subscriber that the Button has been pressed.
     * Every subscriber to Button Press starts triggering their own functions, in this case, remaking the list of current armor, that are subscribed to the Button Pressed event
     */

    //Cool, now an event that carries information
    //There is a slight difference

    //Same way to make a deleate except it has arguments. Also referred to as the delegate's "signature"
    public delegate void ButtonPressed(string s);

    //The same way to make an event
    public static event ButtonPressed OnButtonPressed;

    //Trigger now also has arguments. Your usual function with args. Since our event is type of a delegate that has arguments, you must now include parameters when triggering the event.
    public void TriggerInformation(string s)
    {
        if (OnButtonPressed!= null) //Checking if there are subsribers
        {
            OnButtonPressed(s); //The author will pass the data in the parameter which will passed onto here.
        }
    }

    //Again, you can shorten is as:
    //public void TriggerInformation(string s) => OnButtonPressed?.Invoke(s);

    //So how does this look? I will use functions you are familiar again with to make it clear.
    //In this case, I want to pass the type of amrmor that has been changed in the event.

    //This is ChooseArmor.cs
    //public enum PartType { Weapon, Head, Torso, Bottom };
    //public PartType currentType;
    //..
    //..
    //public void NextOption()
    //{
    //This function's own stuff.
    //..
    //..
    //Then you use the trigger function like any other function to trigger. We pass the type of armor with the trigger.
    //EventManager.instance.TriggerNoInformation(currentType.ToString());
    //}

    //Event is triggered and the EventManager.cs sees it, informs subscribers

    //How to subscribe: The exact same way, however, the subscribed function must match the signature of the delegate!
    //PrinceStats.cs
    //void OnEnable()
    //{
    //  !!!!!!! NO () AFTER THE FUNCTION
    //  EventManager.OnButtonPressed += AddArmor; 
    //}

    //void OnDisable()
    //{
    //  !!!!!!! NO () AFTER THE FUNCTION
    //  EventManager.OnButtonPressed -= AddArmor;
    //}
    //..
    //..
    //void AddArmor(string s) -> matches the delegate arguments
    //{
    //  Script's own function
    //  ..
    //  ..
    //  Debug.Log(s);
    //}

    /*What happens:
     * When player presses Next Option button, the event gets triggered and the author also passes the information with it.
     * EventManager registers that the Button has been pressed and receives the information.
     * EventManagers informs every subscriber that the Button has been pressed along with the information.
     * Every subscriber to Button Press starts triggering their own functions, in this case, remaking the list of current armor, that are subscribed to the Button Pressed event. They can also use the information they received if they need it.
     */
    #endregion

    //So the actual thing and I will pass the type just to demonstrate.
    //You will find the other codes in ChooseArmor.cs and PrinceStats.cs

    public delegate void ArmorChanged(string s);
    public static event ArmorChanged OnArmorChanged;
    public void TR_ArmorChanged(string s) => OnArmorChanged?.Invoke(s);
}
