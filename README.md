# Logrocket for Xamarin.Android

## Installation

LogRocket packages

Nuget: Logrocket.Xamarin.Android


# Step 1 — Adding references to LogRocket

 If you are a new LogRocket user, you’ll need to create an account and start your free trial period.


## Android

 **Permissions**


 We include the INTERNET permission by default as we need it to make network requests:


```xml
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
<uses-permission android:name="android.permission.INTERNET" />
```
# Step 2 — Initialize LogRocket

 First, you’ll need to get your LogRocket app ID. 


First, add the key to the AppID property in the SDKConfig object.
```c#
SDKConfig config = new SDKConfig
{
    AppID = "<APP_SLUG>"
};
```
Then call the static Init method from the SDKInit class.
```c#
SDKInit.Init(this.Application, BaseContext, config);
```
# Step 3 — Create a user

 Finally, you’ll need to create a user, like this:


Add user information to the dictionary. You can get more information here Identify Users.
```c#
IDictionary<string, string> userData = new Dictionary<string, string>
{
    { "name", "Jane Smith" },
    { "email", "janesmith@gmail.com" },
    { "subscriptionPlan", "premium" }
};
```
It remains only to initialize the user.
```c#
SDK.Identify("<USER_ID>", userData);
```
That’s it — now you’ve got a working LogRocket app. However, you will need to register your users before you can receive analytics in your app. For more information, see the section LogRocket for Android.


The full version of LogRocket installing guide is available here — https://docs.logrocket.com/reference/getting-started-with-sdks 


By **Dzmitry Blotski** from **Igniscor**


**Xamarin**

**LogRocket**

**Nuget**

**Libraries**

**Xamarin Forms**
