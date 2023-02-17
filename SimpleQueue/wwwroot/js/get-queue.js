"use strict";
////export interface UserInQueueViewModel {
////    UserId: string;
////    UserInQueueId: string;
////}
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (g && (g = 0, op[0] && (_ = 0)), _) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
var signalR = require("@microsoft/signalr");
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/queue/hub")
    .build();
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
    var userField = document.getElementById('user-' + idParticipant);
    userField.remove();
}
function leaveQueue(idQueue, idParticipant) {
    var uri = "https://localhost:7147/api/queue/".concat(idQueue, "/participant/").concat(idParticipant);
    var method = "post";
    api(apiEndpointUri, method, uri).then(function (response) {
        if (response.ok) {
            deleteUserFromQueue(idQueue, idParticipant);
            afterLeave();
        }
        else
            throw new Error(response.statusText);
    });
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
    return __awaiter(this, void 0, void 0, function () {
        var uri, method, user;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0:
                    uri = "https://localhost:7147/api/queue/".concat(idQueue, "/enter");
                    method = "post";
                    return [4 /*yield*/, request(apiEndpointUri, method, uri)];
                case 1:
                    user = _a.sent();
                    afterJoin();
                    changeLeaveButton(user.queueId, user.userInQueueId);
                    cloneUserElement(user.queueId, user.userInQueueId);
                    return [2 /*return*/];
            }
        });
    });
}
function changeLeaveButton(queueId, userInQueueId) {
    document.getElementById('leave-queue-button')
        .setAttribute('onclick', "leaveQueue('".concat(queueId, "', '").concat(userInQueueId, "')"));
}
function cloneUserElement(queueId, userInQueueId) {
    var elem = document.querySelector('.user-position:last-child');
    var clone = elem.cloneNode(true);
    elem.id = "user-" + userInQueueId;
    elem.getElementsByClassName('delete-user')[0]
        .getElementsByTagName('input')[0]
        .setAttribute('onclick', "leaveQueue('".concat(queueId, "', '").concat(userInQueueId, "')"));
    var username = document.getElementsByClassName('user-profile')[0];
    console.log(username.textContent);
    elem.getElementsByClassName('user-name')[0].innerHTML = username.textContent;
    elem.before(clone);
}
function request(url, method, uri) {
    url = prepareRequest(url, method, uri);
    return fetch(url)
        .then(function (response) { return response.json(); })
        .then(function (data) { return data; });
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
    return fetch(url);
}
function prepareRequest(url, method, uri) {
    return url + "?" + "method=" + method + "&uri=" + uri;
}
//# sourceMappingURL=get-queue.js.map