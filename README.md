# HiGame - Methodology about Managing Mobile Service Dependencies in one codebase for Unity

This demo unity project is developed to present methodology about Managing Mobile Service Dependencies in one codebase.

#### Introduction
I want to present one basic methodology which lets us manage multiple mobile service dependencies in one code base with minimum effort. After Huawei Mobile Services born into the ecosystem, game developers have to maintain one more game version, if they want to reach players which has Huawei mobile phone. 

Actually, Unity developers are used to managing game flows and services according to platforms such as IOS & Android. But this time, there is one thing different. Huawei Mobile phones have still used Android Operations System and your Unity Project should be using the Android Platform when you build for Huawei. But at the end of the day, you have to release 2 different Android APK or bundle for Huawei App Gallery.

#### So, what will be differences between these 2 apks?
irst and the main difference is mobile services. You cannot use GMS in new Huawei Mobile Phones. Because of this, you need to create one APK which includes Huawei Mobile Services to release it in Huawei App Gallery, and one more APK which includes Google Mobile Services to release it in Google Play Market. Services are can be "In-App Purchase", "Advertisement", "Game Services", "Analytics" etc…

Second and the important difference is the package name. You know, both ecosystem works on Android, so to avoid inconsistency and overriding, Huawei App Gallery has one more rule that your package name should end with ".huawei" or ".HUAWEI". This approach just using for separating two ecosystems from each other.

> **Don't worry so much, we can handle these differences in one code base to keep development effort minimum.**

Check the project to see details in code.

If you want to read there you can read this [blog post](https://medium.com/@msalihkarakasli/methodology-about-managing-mobile-service-dependencies-in-one-codebase-for-unity-5878fa2de0a6) about application.

### Used Libraries for this project

1. [Huawei Mobile Services Plugin for Unity](https://github.com/EvilMindDevs/hms-unity-plugin) for Huawei Game Services, Huawei Ads, Huawei IAP Plugin For Unity IAP
2. [Play Game Services Plugin for Unity](https://github.com/playgameservices/play-games-plugin-for-unity) for Google Game Services
3. [Google Mobile Ads Plugin for Unity](https://github.com/googleads/googleads-mobile-unity) for Google Ads
4. [Unity IAP](https://docs.unity3d.com/Manual/UnityIAP.html) for Unity Ads and Google IAP Plugin for Unity IAP

