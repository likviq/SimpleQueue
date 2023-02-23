//import * as signalR from "@microsoft/signalr"

let groupName = setGroupName();

function setGroupName() {
    const queueIdInput = document.getElementById(
        'queue-id',
    ) as HTMLInputElement | null;

    return queueIdInput.value;
}

//@ts-ignore
let connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub/queue")
    .build();

connection.on("increaseClicker", function (data) {
    increaseClickerNumber()
});

connection.on("nextUser", function (username) {
    deleteFirstUserInQueue();

    decreaseAmountOfParticipant();

    const usernameElem = document.getElementsByClassName('user-profile')[0] as HTMLDivElement;
    const currentUsername = usernameElem.textContent.replace(/ /g, '').trim();

    username = username.replace(/ /g, '').trim();

    if (currentUsername == username) {
        afterLeave();
    }
});

connection.on("enterQueue", function (queueId, userInQueueId, username) {
    
    cloneUserElement(queueId, userInQueueId, username);

    increaseAmountOfParticipant();
});

connection.on("leaveQueue", function (queueId, userInQueueId, userPosition) {

    deleteUserFromQueue(queueId, userInQueueId);

    decreaseAmountOfParticipant();

    let currentUserPosition = document.getElementsByClassName(
        'position-user'
    )[0] as HTMLDivElement;
    let currentUserPositionValue = Number(currentUserPosition.textContent)

    if (userPosition < currentUserPositionValue) {
        try {
            decreaseUserPosition()
        }
        catch {

        }
    }
    
});

connection.on("freezeQueue", function () {

    changeFreezeIcon();
});

connection.start()
    .then(function () {
        connection.invoke("addToGroup", groupName)
            .then(function () {
                console.log("user successfully connected");
            }).catch(function (err) {
                return console.error(err.toString());
            })
    });

const apiEndpointUri = "https://localhost:7253/api/callapi";

let isFrozen = false;
setFreezeVariable();

let amountOfParticipant = 0;
setAmountOfParticipant();

let clickerValue = 0;

function clicker() {
    
    let number = connection.invoke("Clicker", groupName, clickerValue);
}

function increaseClickerNumber() {
    clickerValue = clickerValue + 1;

    let clickerNumber = document.getElementById(
        'clicker-value',
    ) as HTMLInputElement | null;

    clickerNumber.textContent = clickerValue.toString();
}

function nextUser(idQueue: string) {
    const uri = `https://localhost:7147/api/queue/${idQueue}/next`;
    const method = "post";

    api(apiEndpointUri, method, uri).then(function () {
        const firstUser = document.getElementsByClassName(
            'user-position',
        )[0] as HTMLInputElement | null;

        const username = firstUser.getElementsByClassName("user-name")[0].textContent;

        connection.invoke("NextUser", groupName, username);
    });    
}

function deleteFirstUserInQueue() {
    const firstUser = document.getElementsByClassName(
        'user-position',
    )[0] as HTMLInputElement | null;

    firstUser.remove();
}

function deleteUserFromQueue(idQueue: string, idParticipant: string) {

    const userField = document.getElementById(
        'user-' + idParticipant,
    ) as HTMLInputElement | null;

    userField.remove();
}

function leaveQueue(idQueue: string, idParticipant: string) {
    const uri = `https://localhost:7147/api/queue/${idQueue}/participant/${idParticipant}`;
    const method = "post";

    api(apiEndpointUri, method, uri).then((response) => {
        if (response.ok) {
            afterLeave();

            let currentUserPosition = document.getElementsByClassName(
                'position-user'
            )[0] as HTMLDivElement;
            let currentUserPositionValue = Number(currentUserPosition.textContent)

            connection.invoke("LeaveQueue", groupName, idQueue, idParticipant, currentUserPositionValue);
        }
        else throw new Error(response.statusText)
    });

    
}

function afterLeave() {

    const enterButton = document.getElementById(
        'join-queue-button',
    ) as HTMLButtonElement | null;
    enterButton.style.display = '';

    const myPositionNull = document.getElementById(
        'your-position-is-null',
    ) as HTMLDivElement | null;
    myPositionNull.style.display = '';

    const leaveButton = document.getElementById(
        'leave-queue-button',
    ) as HTMLButtonElement | null;
    leaveButton.style.display = 'none';

    const myPositionNotNull = document.getElementById(
        'your-position-is-not-null',
    ) as HTMLDivElement | null;
    myPositionNotNull.style.display = 'none';
}

async function enterQueue(idQueue: string) {
    const uri = `https://localhost:7147/api/queue/${idQueue}/enter`;
    const method = "post";

    let user: User;
    user = await request<User>(apiEndpointUri, method, uri);

    const usernameElem = document.getElementsByClassName('user-profile')[0] as HTMLDivElement;
    const username = usernameElem.textContent;

    connection.invoke("EnterQueue", groupName, user.queueId, user.userInQueueId, username);

    afterJoin();

    changeLeaveButton(user.queueId, user.userInQueueId);
}

function changeLeaveButton(queueId: string, userInQueueId: string) {
    document.getElementById('leave-queue-button')
        .setAttribute('onclick', `leaveQueue('${queueId}', '${userInQueueId}')`);
}

function cloneUserElement(queueId: string, userInQueueId: string, username: string) {
    let elem = document.querySelector('.user-position:last-child');

    let clone = elem.cloneNode(true);

    elem.id = "user-" + userInQueueId;

    elem.getElementsByClassName('delete-user')[0]
        .getElementsByTagName('input')[0]
        .setAttribute('onclick', `leaveQueue('${queueId}', '${userInQueueId}')`);

    elem.getElementsByClassName('user-name')[0].innerHTML = username;

    elem.before(clone);
}

interface User {
    userId: string;
    queueId: string;
    userInQueueId: string;
}

function request<TResponse>(url: string, method: string, uri: string): Promise<TResponse> {

    url = prepareRequest(url, method, uri);

    return fetch(url)
        .then(function (response){
            console.log(response.headers.get("Content-Type"));
            return response.json()
        })
        .then((data) => data as TResponse);

}

function afterJoin() {

    const enterButton = document.getElementById(
        'join-queue-button',
    ) as HTMLInputElement | null;
    enterButton.style.display = 'none';

    const myPositionNull = document.getElementById(
        'your-position-is-null',
    ) as HTMLInputElement | null;
    myPositionNull.style.display = 'none';

    const leaveButton = document.getElementById(
        'leave-queue-button',
    ) as HTMLInputElement | null;
    leaveButton.style.display = '';

    const myPositionNotNull = document.getElementById(
        'your-position-is-not-null',
    ) as HTMLInputElement | null;
    myPositionNotNull.style.display = '';
    myPositionNotNull.querySelector('.position-user').textContent = (amountOfParticipant + 1).toString();
}

function decreaseAmountOfParticipant() {
    let participants = document.getElementById(
        'admin-number',
    ) as HTMLInputElement | null;

    amountOfParticipant -= 1;

    participants.textContent = amountOfParticipant.toString();
}

function decreaseUserPosition() {
    let currentUserPosition = document.getElementsByClassName(
        'position-user'
    )[0] as HTMLDivElement;

    currentUserPosition.textContent = (Number(currentUserPosition.textContent) - 1).toString();
}

function increaseAmountOfParticipant() {
    let participants = document.getElementById(
        'admin-number',
    ) as HTMLInputElement | null;

    amountOfParticipant += 1;

    participants.textContent = amountOfParticipant.toString();
}

function freezeQueue(id: string) {
    const uri = "https://localhost:7147/api/queue/" + id;
    const method = "post";

    api(apiEndpointUri, method, uri).then((response) => {
        if (response.ok) {
            connection.invoke("FreezeQueue", groupName);

            changeFreezeButton();
        }
        else throw new Error(response.statusText)
    });   
}

function changeFreezeButton() {
    const freezeImage = document.getElementById(
        'freeze-button',
    ) as HTMLInputElement | null;

    const unFreezeImage = document.getElementById(
        'unfreeze-button',
    ) as HTMLInputElement | null;

    if (!isFrozen == true) {
        freezeImage.style.display = 'none';
        unFreezeImage.style.display = '';
    }
    else {
        freezeImage.style.display = '';
        unFreezeImage.style.display = 'none';
    }

}

function changeFreezeIcon() {
    isFrozen = !isFrozen;

    const statusQueueImage = document.getElementById(
        'status-queue-image',
    ) as HTMLInputElement | null;

    if (isFrozen == true) {
        statusQueueImage.style.backgroundColor = ''
    }
    else {
        statusQueueImage.style.backgroundColor = '#82FF9D'
    }
}

function setFreezeVariable() {
    const statusQueueImage = document.getElementById(
        'status-queue-image',
    ) as HTMLInputElement | null;

    const imageColor = statusQueueImage.style.backgroundColor;

    if (imageColor == '') {
        isFrozen = true;
    }
}

function setAmountOfParticipant() {
    let participants = document.getElementById(
        'admin-number',
    ) as HTMLInputElement | null;

    amountOfParticipant = parseInt(participants.textContent);
}

function api(url: string, method: string, uri: string) {
    url = prepareRequest(url, method, uri);

    return fetch(url);
}

function prepareRequest(url: string, method: string, uri: string): string {
    return url + "?" + "method=" + method + "&uri=" + uri;
}