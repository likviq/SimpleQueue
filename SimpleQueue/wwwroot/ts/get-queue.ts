////export interface UserInQueueViewModel {
////    UserId: string;
////    UserInQueueId: string;
////}

const apiEndpointUri = "https://localhost:7253/api/callapi";

let isFrozen = false;
setFreezeVariable();

let amountOfParticipant = 0;
setAmountOfParticipant();

let clickerValue = 0;

function clicker() {
    clickerValue = clickerValue + 1;

    let clickerNumber = document.getElementById(
        'clicker-value',
    ) as HTMLInputElement | null;
    
    clickerNumber.textContent = clickerValue.toString();
}

function nextUser(idQueue: string) {
    const uri = `https://localhost:7147/api/queue/${idQueue}/next`;
    const method = "post";

    api(apiEndpointUri, method, uri);

    const firstUser = document.getElementsByClassName(
        'user-position',
    )[0] as HTMLInputElement | null;

    firstUser.remove();

    decreaseAmountOfParticipant();
}

function deleteUserFromQueue(idQueue: string, idParticipant: string) {
    const uri = `https://localhost:7147/api/queue/${idQueue}/participant/${idParticipant}`;
    const method = "post";

    api(apiEndpointUri, method, uri);

    const userField = document.getElementById(
        'user-' + idParticipant,
    ) as HTMLInputElement | null;

    userField.remove();
}

function leaveQueue(idQueue: string, idParticipant: string) {
    deleteUserFromQueue(idQueue, idParticipant);

    afterLeave();
}

function afterLeave() {
    decreaseAmountOfParticipant();

    const enterButton = document.getElementById(
        'join-queue-button',
    ) as HTMLInputElement | null;
    enterButton.style.display = '';

    const myPositionNull = document.getElementById(
        'your-position-is-null',
    ) as HTMLInputElement | null;
    myPositionNull.style.display = '';

    const leaveButton = document.getElementById(
        'leave-queue-button',
    ) as HTMLInputElement | null;
    leaveButton.style.display = 'none';

    const myPositionNotNull = document.getElementById(
        'your-position-is-not-null',
    ) as HTMLInputElement | null;
    myPositionNotNull.style.display = 'none';
}

function enterQueue(idQueue: string) {
    const uri = `https://localhost:7147/api/queue/${idQueue}/enter`;
    const method = "post";

    let viewModel = apiEnterQueue(apiEndpointUri, method, uri);
    alert(viewModel);

    afterJoin();

    cloneUserElement("asdasdasd");
}

function cloneUserElement(userInQueueId: string) {
    var elem = document.querySelector('.user-position:last-child') as HTMLDivElement;
    elem.id = "user-" + userInQueueId;

    var clone = elem.cloneNode(true);

    elem.after(clone);
}

function apiEnterQueue(url: string, method: string, uri: string): Promise<string> {
    url = prepareRequest(url, method, uri);

    return fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error(response.statusText)
            }

            console.log(response.text);
            return response.json()
        })
        .then(data => {
            return data as string
        });
}

function afterJoin() {
    increaseAmountOfParticipant();

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
    myPositionNotNull.querySelector('.position-user').textContent = amountOfParticipant.toString();
}

function decreaseAmountOfParticipant() {
    let participants = document.getElementById(
        'admin-number',
    ) as HTMLInputElement | null;

    amountOfParticipant -= 1;

    participants.textContent = amountOfParticipant.toString();
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

    api(apiEndpointUri, method, uri);

    isFrozen = !isFrozen;

    const statusQueueImage = document.getElementById(
        'status-queue-image',
    ) as HTMLInputElement | null;

    const freezeImage = document.getElementById(
        'freeze-button',
    ) as HTMLInputElement | null;

    const unFreezeImage = document.getElementById(
        'unfreeze-button',
    ) as HTMLInputElement | null;

    if (isFrozen == true) {
        freezeImage.style.display = 'none';
        unFreezeImage.style.display = '';

        statusQueueImage.style.backgroundColor = ''
    }
    else {
        freezeImage.style.display = '';
        unFreezeImage.style.display = 'none';

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

function api<T>(url: string, method: string, uri: string): Promise<T> {
    url = prepareRequest(url, method, uri);

    return fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error(response.statusText)
            }

            console.log(response.text);
            return response.json() as Promise<{ data: T }>
        })
        .then(data => {
            console.log(data.data)
            return data.data
        })
}

function prepareRequest(url: string, method: string, uri: string): string {
    return url + "?" + "method=" + method + "&uri=" + uri;
}