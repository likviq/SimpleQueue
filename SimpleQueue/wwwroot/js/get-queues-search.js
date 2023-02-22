var myHeaders = new Headers();
console.log(myHeaders.get("pagination"));
var QueueParameters = /** @class */ (function () {
    function QueueParameters() {
        this.StartTime = "";
        this.EndTime = "";
        this.SearchTerm = "";
        this.IsFrozen = null;
        this.IsChat = null;
        this.IsProtected = null;
    }
    return QueueParameters;
}());
var queueParams = new QueueParameters();
setQueueParam();
function setQueueParam() {
    var searchValue = document.getElementById('search-value');
    queueParams.SearchTerm = searchValue.value;
    var startTimeValue = document.getElementById('start-time-value');
    queueParams.StartTime = startTimeValue.value;
    var endTimeValue = document.getElementById('end-time-value');
    queueParams.EndTime = endTimeValue.value;
}
function search() {
    var searchValue = document.getElementById('search-value');
    queueParams.SearchTerm = searchValue.value;
    console.log(queueParams.SearchTerm);
}
function startTime() {
    var startTimeValue = document.getElementById('start-time-value');
    queueParams.StartTime = startTimeValue.value;
    console.log(queueParams.StartTime);
}
function endTime() {
    var endTimeValue = document.getElementById('end-time-value');
    queueParams.EndTime = endTimeValue.value;
    console.log(queueParams.EndTime);
}
function freeze() {
    var freezeValue = document.getElementById('freeze-value');
    queueParams.IsFrozen = getBooleanValue(freezeValue.value);
    console.log(queueParams.IsFrozen);
}
function chat() {
    var chatValue = document.getElementById('chat-value');
    queueParams.IsChat = getBooleanValue(chatValue.value);
    console.log(queueParams.IsChat);
}
function privacy() {
    var privacyValue = document.getElementById('privacy-value');
    queueParams.IsProtected = getBooleanValue(privacyValue.value);
    console.log(queueParams.IsProtected);
}
function getBooleanValue(value) {
    if (value == "") {
        return null;
    }
    return Boolean(Number(value));
}
var urlQueues = "https://localhost:7253/queues";
function findQueues() {
    console.log(queueParams);
    var url = urlQueues +
        "?StartTime=".concat(queueParams.StartTime, "&EndTime=").concat(queueParams.EndTime, "&SearchTerm=").concat(queueParams.SearchTerm, "&IsFrozen=").concat(queueParams.IsFrozen, "&IsChat=").concat(queueParams.IsChat, "&IsProtected=").concat(queueParams.IsProtected);
    window.location.href = url;
}
//# sourceMappingURL=get-queues-search.js.map