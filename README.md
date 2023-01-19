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
