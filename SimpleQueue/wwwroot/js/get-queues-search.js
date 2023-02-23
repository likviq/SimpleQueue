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
var myHeaders = new Headers();
console.log(myHeaders.get("pagination"));
var pageNumber = 1;
var pageSize = 10;
var QueueParameters = /** @class */ (function () {
    function QueueParameters() {
        this.StartTime = "";
        this.EndTime = "";
        this.SearchTerm = "";
        this.IsFrozen = null;
        this.IsChat = null;
        this.IsProtected = null;
        this.PageNumber = pageNumber;
        this.PageSize = pageSize;
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
        "?StartTime=\n".concat(queueParams.StartTime, "&EndTime=").concat(queueParams.EndTime, "&SearchTerm=\n").concat(queueParams.SearchTerm, "&IsFrozen=").concat(queueParams.IsFrozen, "&IsChat=\n").concat(queueParams.IsChat, "&IsProtected=").concat(queueParams.IsProtected);
    window.location.href = url;
}
function uploadMoreQueues() {
    return __awaiter(this, void 0, void 0, function () {
        var queues, i, elem, clone;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, findMoreQueues()];
                case 1:
                    queues = _a.sent();
                    console.log(queues);
                    console.log(document.querySelector(".queue-body"));
                    for (i = 0; i < queues.length; i++) {
                        elem = document.querySelector('.queue-body:last-of-type');
                        clone = elem.cloneNode(true);
                        elem.setAttribute('href', '/queue/' + queues[i].id);
                        elem.getElementsByClassName('queue-title')[0]
                            .innerHTML = queues[i].title;
                        elem.getElementsByClassName('queue-description')[0]
                            .innerHTML = queues[i].description;
                        elem.before(clone);
                    }
                    return [2 /*return*/];
            }
        });
    });
}
function hideMoreButton() {
    var moreButton = document.getElementById('more-button');
    moreButton.style.display = 'none';
}
function findMoreQueues() {
    queueParams.PageNumber += 1;
    var url = prepareUrl();
    var encodedUrl = encodeURI(url);
    return fetch(encodedUrl)
        .then(function (response) {
        if (response.ok) {
            hideMoreButton();
            console.log(response.headers.get("pagination"));
            return response.json();
        }
        throw new Error(response.statusText);
    })
        .then(function (data) { return data; });
}
var urlApiQueues = "https://localhost:7147/api/queues";
function prepareUrl() {
    var startTimeQuery = queueParams.StartTime == "" ? "" : "&StartTime=".concat(queueParams.StartTime);
    var endTimeQuery = queueParams.EndTime == "" ? "" : "&EndTime=".concat(queueParams.EndTime);
    var searchTermQuery = queueParams.SearchTerm == "" ? "" : "&SearchTerm=".concat(queueParams.SearchTerm);
    var isFrozenQuery = queueParams.IsFrozen == null ? "" : "&IsFrozen=".concat(queueParams.IsFrozen);
    var isChatQuery = queueParams.IsChat == null ? "" : "&IsChat=".concat(queueParams.IsChat);
    var isProtectedQuery = queueParams.IsProtected == null ? "" : "&IsProtected=".concat(queueParams.IsProtected);
    return urlApiQueues +
        "?PageNumber=".concat(queueParams.PageNumber, "&PageSize=").concat(queueParams.PageSize)
        + startTimeQuery + endTimeQuery + searchTermQuery
        + isFrozenQuery + isChatQuery + isProtectedQuery;
}
//# sourceMappingURL=get-queues-search.js.map