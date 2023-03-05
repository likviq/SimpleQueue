## SimpleQueue
SimpleQueue - система створення черг для різних подій, як онлайн та і офлайн. Приєднуватись можна буде за допомогою лінка, в якому вже буде захешований пароль, або не захешований - буде видно. Це посилання можна отримати або від когось, хто вже у черзі і може вам його надіслати через особисті повідомлення на цьому ж сайті, або  через QrCode(створення цього коду буде доступно для власника черги) - в нього буде зашито те саме посилання з паролем, якщо власник, звісно, його встановив. Учасники черги можуть мати можливість переписуватись між собою у спільному чаті(тут я планую підключити функціонал веб-сокетів і не тільки тут), також вони можуть змінювати своє місце в черзі з іншою людиною. Планую ще зробити, щоб веб-додаток пропонував користувачеві найкращі події відповідно до його даних в базі, такий собі мікро штучний інтелект(лінійна регресія), яка матиме дані користувача на вході і найкращі події на виході. 
Якщо коротко, то задумка полягає в тому, щоб зробити міні соцмережу(з особистою сторінкою та постами) з основним функціоналом, який полягає у функціональній частині черг для різних подій - чи то черга на здачу лабораторних, чи на прийом до лікаря.
Features:
- queue creating with google api(get string address from latitude and longitude), with password(optional);
- every queue can have his own chat(also optional);
- each queue will have custom qrCode to join;
- creator can freeze, change start time and kick members
- people in queue can swap their places with each other;
- to communicate about swapping places users can use private chat;
- each user will have his personal page with some statuses - some kind of mini social network;
- global error handling;
- different error pages(500, 404, 400);
- automapper for DTOs and VeiwModels.

##### Upon entering the site, the user will see the main page with brief information about the application in English, but the user can change it to Ukrainian:
![homePage](https://user-images.githubusercontent.com/80317021/222966817-a9f1ad57-de39-48a5-a612-f7c5b2b274ef.gif)
##### In order to create an account, the user needs to go to the registration page and fill in all the required fields:
- unique username;
- valid email address;
- password;
- confirm password (passwords must match):
![registrationpage](https://user-images.githubusercontent.com/80317021/222968160-22a3e69e-434b-4a20-987a-1a97accdd8d5.gif)
##### The user who has an account can log in with a username and password:
![loginpage](https://user-images.githubusercontent.com/80317021/222968713-f118782e-c6dc-4d1e-a2f7-6db87f9d6f6e.gif)
##### You can also use your Google or Facebook account data to create a new user and login
![googleAuth](https://user-images.githubusercontent.com/80317021/222969213-04ec3457-4b5a-432f-98ee-e4215e0ace98.gif)
##### Each user (registered and not) can find the queue he needs according to the following parameters:
- Search value (the search is carried out by finding the same substrings, which can be supplemented with tags)
- Start date
- End date
- Is it frozen?
- Is there a chat for communication?
- Is there a password to access the queue
![searchPage](https://user-images.githubusercontent.com/80317021/222969778-bea8ef56-812e-4bc8-b24d-1d96cff078f5.gif)
##### The users can also sort received queues according to the following criteria:
- Recommended;
- The newest;
- The oldest;
- More popular first (more users in the queue);
- The least popular.
![sortby](https://user-images.githubusercontent.com/80317021/222971117-a9988026-c3f6-4264-ae0f-7e7cc349cc15.gif)
##### There are only two types of queues:
- fast (in real time, users join and do not have a constant value when it will be their turn);
- postponed (the queue owner chooses the date and the amount of time required for one user).
##### Each queue can have a picture that every user will see. All images are stored in azure storage.
![Вигляд створення черг](https://user-images.githubusercontent.com/80317021/222973157-b225e283-e0bf-4678-b65d-eed5c003bb00.png)
![Вигляд черг](https://user-images.githubusercontent.com/80317021/222973068-6a09cc3f-5e5d-44e9-a2cb-c28df1d0a22b.png)
##### Delayed queue:
Each user can take several places, all operations that change the UI don't update the entire page, but only replace elements dynamically for each user using the signalR. For instance, if someone took a place, then another user will see it without reloading the entire page.
![delayedQueue](https://user-images.githubusercontent.com/80317021/222974055-5540c2d1-16f3-4296-b966-dc730ceb87a6.gif)
Any user can check the link to the queue by clicking on the button, or open the page for printing the code. There is also a small qr code on the queue page itself, which contains information for users how to open that page. All generated qr codes are not stored anywhere, but generated dynamically, according to the link.
![qrCodeSample](https://user-images.githubusercontent.com/80317021/222974590-586edc11-c11d-4a00-8388-c7d08f38cef5.gif)
