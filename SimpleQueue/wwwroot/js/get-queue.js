////export interface UserInQueueViewModel {
////    UserId: string;
////    UserInQueueId: string;
////}
var apiEndpointUri = "https://localhost:7253/api/callapi";
var isFrozen = false;
setFreezeVariable();
var amountOfParticipant = 0;
setAmountOfParticipant();
var clickerValue = 0;
function clicker() {
    clickerValue = clickerValue + 1;
    var clickerNumber = document.getElementById('clicker-value');
    clickerNumber.textContent = clickerValue.toString();
}
function nextUser(idQueue) {
    var uri = "https://localhost:7147/api/queue/".concat(idQueue, "/next");
    var method = "post";
    api(apiEndpointUri, method, uri);
    var firstUser = document.getElementsByClassName('user-position')[0];
    firstUser.remove();
    decreaseAmountOfParticipant();
}
function deleteUserFromQueue(idQueue, idParticipant) {
    var uri = "https://localhost:7147/api/queue/".concat(idQueue, "/participant/").concat(idParticipant);
    var method = "post";
    api(apiEndpointUri, method, uri);
    var userField = document.getElementById('user-' + idParticipant);
    userField.remove();
}
function leaveQueue(idQueue, idParticipant) {
    deleteUserFromQueue(idQueue, idParticipant);
    afterLeave();
}
function afterLeave() {
    decreaseAmountOfParticipant();
    var enterButton = document.getElementById('join-queue-button');
    enterButton.style.display = '';
    var myPositionNull = document.getElementById('your-position-is-null');
    myPositionNull.style.display = '';
    var leaveButton = document.getElementById('leave-queue-button');
    leaveButton.style.display = 'none';
    var myPositionNotNull = document.getElementById('your-position-is-not-null');
    myPositionNotNull.style.display = 'none';
}
function enterQueue(idQueue) {
    var uri = "https://localhost:7147/api/queue/".concat(idQueue, "/enter");
    var method = "post";
    var viewModel = apiEnterQueue(apiEndpointUri, method, uri);
    alert(viewModel);
    afterJoin();
    cloneUserElement("asdasdasd");
}
function cloneUserElement(userInQueueId) {
    var elem = document.querySelector('.user-position:last-child');
    elem.id = "user-" + userInQueueId;
    var clone = elem.cloneNode(true);
    elem.after(clone);
}
function apiEnterQueue(url, method, uri) {
    url = prepareRequest(url, method, uri);
    return fetch(url)
        .then(function (response) {
        if (!response.ok) {
            throw new Error(response.statusText);
        }
        console.log(response.text);
        return response.json();
    })
        .then(function (data) {
        return data;
    });
}
function afterJoin() {
    increaseAmountOfParticipant();
    var enterButton = document.getElementById('join-queue-button');
    enterButton.style.display = 'none';
    var myPositionNull = document.getElementById('your-position-is-null');
    myPositionNull.style.display = 'none';
    var leaveButton = document.getElementById('leave-queue-button');
    leaveButton.style.display = '';
    var myPositionNotNull = document.getElementById('your-position-is-not-null');
    myPositionNotNull.style.display = '';
    myPositionNotNull.querySelector('.position-user').textContent = amountOfParticipant.toString();
}
function decreaseAmountOfParticipant() {
    var participants = document.getElementById('admin-number');
    amountOfParticipant -= 1;
    participants.textContent = amountOfParticipant.toString();
}
function increaseAmountOfParticipant() {
    var participants = document.getElementById('admin-number');
    amountOfParticipant += 1;
    participants.textContent = amountOfParticipant.toString();
}
function freezeQueue(id) {
    var uri = "https://localhost:7147/api/queue/" + id;
    var method = "post";
    api(apiEndpointUri, method, uri);
    isFrozen = !isFrozen;
    var statusQueueImage = document.getElementById('status-queue-image');
    var freezeImage = document.getElementById('freeze-button');
    var unFreezeImage = document.getElementById('unfreeze-button');
    if (isFrozen == true) {
        freezeImage.style.display = 'none';
        unFreezeImage.style.display = '';
        statusQueueImage.style.backgroundColor = '';
    }
    else {
        freezeImage.style.display = '';
        unFreezeImage.style.display = 'none';
        statusQueueImage.style.backgroundColor = '#82FF9D';
    }
}
function setFreezeVariable() {
    var statusQueueImage = document.getElementById('status-queue-image');
    var imageColor = statusQueueImage.style.backgroundColor;
    if (imageColor == '') {
        isFrozen = true;
    }
}
function setAmountOfParticipant() {
    var participants = document.getElementById('admin-number');
    amountOfParticipant = parseInt(participants.textContent);
}
function api(url, method, uri) {
    url = prepareRequest(url, method, uri);
    return fetch(url)
        .then(function (response) {
        if (!response.ok) {
            throw new Error(response.statusText);
        }
        console.log(response.text);
        return response.json();
    })
        .then(function (data) {
        console.log(data.data);
        return data.data;
    });
}
function prepareRequest(url, method, uri) {
    return url + "?" + "method=" + method + "&uri=" + uri;
}
//# sourceMappingURL=get-queue.js.map