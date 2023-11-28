const API_URLS = {
    competitor: "competitor",
    category: "category",
    competition: "competition",
    getCompetitorsByBoatCategory: "stat/CompetitorsByBoatCategory",
    getCompetitorWithAllRelevantData: "stat/CompetitorWithAllRelevantData",
    getOpponentsByName: "stat/OpponentsByName",
    getCompetition_BasedOnCompetitorsNameAndNation: "stat/Competition_BasedOnCompetitorsNameAndNation",
    getAvgAge: "stat/AVGAge"
}

const DISPLAY_VALUES = {
    flex: 'flex',
    none: 'none',
    block: 'block'
}
const DEFAULT_HEADERS = { 'Content-Type': 'application/json', }

const XHR_TYPES = {
    get: "GET",
    post: 'POST',
    put: 'PUT',
    delete: 'DELETE'
}

const TAB_UIDS= {
    competitors: "competitors",
    categories: "categories",
    competitions: "competitions",
    otherData: "otherData"
}

const TAB_QUERY_PARAM = 'currentTab';

const ELEMENT_CLASSES = {
    active: "active",
    hidden: 'hidden'
}

const ID_POSFIXES = {
    table: "Table",
    add: 'AddForm',
    edit: 'EditForm',
}


let competitors = [];
let categories = [];
let competitions = [];
let connection = null;


setupSignalR();

let IdToUpdate = -1;
let IdToUpdateCategory = -1;
let IdToUpdateCompetition = -1;


// Handle Tabs

window.addEventListener('load', () => {
    const currentTab = readCurrentTabFromQueryParams();

    if (currentTab) {
        openTab(document.getElementById(`${currentTab}Tab`), currentTab);
        return;
    }

    openTab(document.getElementById('competitorsTab'), TAB_UIDS.competitors);
})

function openTab(evt, tabId) {



    let tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (let i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = DISPLAY_VALUES.none;
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (let i = 0; i < tablinks.length; i++) {
        removeClass(tablinks[i], ELEMENT_CLASSES.active);
    }

    setElelmentVisibilityById(tabId, DISPLAY_VALUES.block);
    setQueryParams(TAB_QUERY_PARAM, tabId);

    if (evt.currentTarget) {
        addClass(evt.currentTarget, ELEMENT_CLASSES.active);
    } else {
        addClass(evt, ELEMENT_CLASSES.active);
    }

    if (tabId == TAB_UIDS.otherData) {
        return;
    }

    switch (tabId) {
        case TAB_UIDS.competitors:
            getdata();
            break
        case TAB_UIDS.categories:
            getdata_category();
            break;
        case TAB_UIDS.competitions:
            getdata_competition();
            break;
    }

    closeAllPanelsAndShowTable(tabId);

}


function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:55475/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("CompetitorCreated", (user, message) => {
        getdata();
    });

    connection.on("CompetitorDeleted", (user, message) => {
        getdata();
    });

    connection.on("CompetitorUpdated", (user, message) => {
        getdata();
    });
    connection.on("CategoryCreated", (user, message) => {
        getdata_category();
    });

    connection.on("CategoryDeleted", (user, message) => {
        getdata_category();
    });

    connection.on("CategoryUpdated", (user, message) => {
        getdata_category();
    });
    connection.on("CompetitionCreated", (user, message) => {
        getdata();
    });

    connection.on("CompetitionDeleted", (user, message) => {
        getdata();
    });

    connection.on("CompetitionUpdated", (user, message) => {
        getdata();
    });

    connection.onclose(async () => {
        await start();
    });
    start();
}

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

// Data init

function getdata() {

    xhrGet(API_URLS.competitor)
        .then(response => response.json())
        .then(data => {
            competitors = data
            display();
        })

}

async function getdata_category() {

    xhrGet(API_URLS.category)
        .then(response => response.json())
        .then(data => {
            categories = data
            display_category();
        })
}

async function getdata_competition() {

    xhrGet(API_URLS.competition)
        .then(response => response.json())
        .then(data => {
            competitions = data
            display_competition();
        })
}

// Display


function display() {
    closeAllPanelsAndShowTable('competitors');
    resetInnerHtmlById('resultarea');
    competitors.forEach(competitior => {
        document.getElementById('resultarea').innerHTML +=
            "<tr><td>" + competitior.id + "</td><td>"
            + competitior.name + "</td><td>"
            + competitior.age + "</td><td>"
            + competitior.competitonID + "</td><td>"
            + competitior.categoryID + "</td><td>"
            + competitior.nation + "</td><td>" +
            `<button type="button" onclick="remove_competitor(${competitior.id})">Delete</button>` +
            `<button type="button" onclick="showupdate_competitor(${competitior.id})">Update</button>`
            + "</td></tr>";
    });
}

function display_category() {
    closeAllPanelsAndShowTable('categories');
    resetInnerHtmlById('categoryresultarea');
    categories.forEach(category => {
        document.getElementById('categoryresultarea').innerHTML +=
            "<tr><td>" + category.categoryNumber + "</td><td>"
        + category.ageGroup + "</td><td>"
        + category.boatCategory + "</td><td>" +
        `<button type="button" onclick="remove_category(${category.categoryNumber})">Delete</button>` +
        `<button type="button" onclick="showupdate_category(${category.categoryNumber})">Update</button>`
            + "</td></tr>";
    });
}

function display_competition() {
    closeAllPanelsAndShowTable('competitions');
    resetInnerHtmlById('competitionresultarea');
    competitions.forEach(competition => {
        document.getElementById('competitionresultarea').innerHTML +=
            "<tr><td>" + competition.id + "</td><td>"
        + competition.competitorID + "</td><td>"
        + competition.opponentID + "</td><td>"
        + competition.numberOfRacesAgainstEachOther + "</td><td>"
        + competition.location + "</td><td>"
        + competition.distance + "</td><td>" +
        `<button type="button" onclick="remove_competition(${competition.id})">Delete</button>` +
        `<button type="button" onclick="showupdate_competition(${competition.id})">Update</button>`
            + "</td></tr>";
    });
}


// Remove


function remove_competitor(id) {
    remove(API_URLS.competitor, id, getdata);
}

function remove_category(categoryNumber) {
    remove(API_URLS.category, categoryNumber, getdata_category);
}

function remove_competition(id) {
    remove(API_URLS.competition, id, getdata_competition);
}

function remove(url, id, successFn) {
    xhrDelete(url, id)
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            successFn();
        })
        .catch((error) => { console.error('Error:', error); });
}

// Update

function showupdate_competitor(id) {


    const currentCompetitor = competitors.find(t => t.id == id);

    setElementValueById('nameToUpdate', currentCompetitor.name);
    setElementValueById('agetoupdate', currentCompetitor.age);
    setElementValueById('competitonIDtoupdate', currentCompetitor.competitonID);
    setElementValueById('categoryIDtoupdate', currentCompetitor.categoryID);
    setElementValueById('nationtoupdate', currentCompetitor.nation);
    edit(TAB_UIDS.competitors);

    IdToUpdate = id;
}

function update_competitor() {
    closeAllPanelsAndShowTable(TAB_UIDS.competitors);

    const request = {
        id: IdToUpdate,
        name: getElementValueById('nameToUpdate'),
        age: getElementValueById('agetoupdate'),
        competitonID: getElementValueById('competitonIDtoupdate'),
        categoryID: getElementValueById('categoryIDtoupdate'),
        nation: getElementValueById('nationtoupdate')
    };

    update(API_URLS.competitor, request, getdata);
}

function showupdate_category(id) {
    edit(TAB_UIDS.categories);

    const category = categories.find(t => t.categoryNumber == id)

    setElementValueById('ageGrouptoupdate', category.ageGroup);
    setElementValueById('boatCategorytoupdate', category.boatCategory);

    //setElelmentVisibilityById('updateformdivcategory', DISPLAY_VALUES.flex);

    IdToUpdateCategory = id;
}

function update_category() {
    closeAllPanelsAndShowTable(TAB_UIDS.categories);

    const request = {
        categoryNumber: IdToUpdateCategory,
        ageGroup: getElementValueById('ageGrouptoupdate'),
        boatCategory: getElementValueById('boatCategorytoupdate')
    }

    update(API_URLS.category, request, getdata_category);

}

function showupdate_competition(id) {

    edit(TAB_UIDS.competitions);

    const competition = competitions.find(t => t.id == id)

    setElementValueById('competitorIDtoupdate', competition.competitorID);
    setElementValueById('opponentIDtoupdate', competition.opponentID);
    setElementValueById('numberOfRacesAgainstEachOthertoupdate', competition.numberOfRacesAgainstEachOther);
    setElementValueById('locationtoupdate', competition.location);
    setElementValueById('distancetoupdate', competition.distance);

    IdToUpdateCompetition = id;
}

function update_competition() {
    closeAllPanelsAndShowTable(TAB_UIDS.competitions);

    let competitorID = document.getElementById('competitorIDtoupdate').value;
    let opponentID = document.getElementById('opponentIDtoupdate').value;
    let numberOfRacesAgainstEachOther = document.getElementById('numberOfRacesAgainstEachOthertoupdate').value;
    let location = document.getElementById('locationtoupdate').value;
    let distance = document.getElementById('distancetoupdate').value;
    fetch('http://localhost:55475/competition', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            { competitorID: competitorID, id: IdToUpdateCompetition, opponentID: opponentID,  numberOfRacesAgainstEachOther: numberOfRacesAgainstEachOther, location: location, distance: distance })
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata_competition();
        })
        .catch((error) => { console.error('Error:', error); });
}

function update(url, request, successFn) {
    xhrPut(url, request)
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            successFn();
        })
        .catch((error) => { console.error('Error:', error); });
}





function create_competitor() {

    const request = {
        name: getElementValueById('name'),
        age: getElementValueById('age'),
        competitonID: getElementValueById('competitonID'),
        categoryID: getElementValueById('categoryID'),
        nation: getElementValueById('nation')
    }

    create(API_URLS.competitor, request, getdata)

}

function create_category() {
    let ageGroup = document.getElementById('ageGroup').value;
    let boatCategory = document.getElementById('boatCategory').value;
    fetch('http://localhost:55475/category', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            { ageGroup: ageGroup, boatCategory: boatCategory })
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata_category();
        })
        .catch((error) => { console.error('Error:', error); });
}

function create_competition() {
    let competitorID = document.getElementById('competitorID').value;
    let opponentID = document.getElementById('opponentID').value;
    let numberOfRacesAgainstEachOther = document.getElementById('numberOfRacesAgainstEachOther').value;
    let location = document.getElementById('location').value;
    let distance = document.getElementById('distance').value;
    fetch('http://localhost:55475/competition', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            { competitorID: competitorID, opponentID: opponentID, numberOfRacesAgainstEachOther: numberOfRacesAgainstEachOther, location: location, distance: distance })
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata_competition();
        })
        .catch((error) => { console.error('Error:', error); });
}

function create(url, request, successFn) {
    xhrPost(url, request)
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            successFn();
        })
        .catch((error) => { console.error('Error:', error); });
}

// Non CRUD

function getCompetitorsByBoatCategory() {
    xhrGet(API_URLS.getCompetitorsByBoatCategory)
        .then(response => response.json())
        .then(data => {
            console.log(data)
            displayCompetitorsByBoatCategory(data)
        })
}
function displayCompetitorsByBoatCategory(data) {
    document.getElementById('competitorsbyboatcategory').innerHTML = JSON.stringify(data)
}

function getCompetitorWithAllRelevantData() {
    xhrGet(API_URLS.getCompetitorWithAllRelevantData)
        .then(response => response.json())
        .then(data => {
            console.log(data)
            displayCompetitionWithAllRelevantData(data)
        })
}
function displayCompetitionWithAllRelevantData(data) {
    document.getElementById('competitorwithallrelevantdata').innerHTML = JSON.stringify(data)
}

function getOpponentsByName() {
    xhrGet(API_URLS.getOpponentsByName)
        .then(response => response.json())
        .then(data => {
            console.log(data)
            displayOpponentsByName(data)
        })
}
function displayOpponentsByName(data) {
    document.getElementById('OpponentsByName').innerHTML = JSON.stringify(data)
}

function getCompetition_BasedOnCompetitorsNameAndNation() {
    xhrGet(API_URLS.getCompetition_BasedOnCompetitorsNameAndNation)
        .then(response => response.json())
        .then(data => {
            console.log(data)
            displayCompetition_BasedOnCompetitorsNameAndNation(data)
        })
}
function displayCompetition_BasedOnCompetitorsNameAndNation(data) {
    document.getElementById('CompetitionBasedOnCompetitorsNameAndNation').innerHTML = JSON.stringify(data)
}

function getAvgAge() {
    xhrGet(API_URLS.getAvgAge)
        .then(response => response.json())
        .then(data => {
            console.log(data)
            displayAvgAge(data)
        })
}
function displayAvgAge(data) {
    document.getElementById('AvgAge').innerHTML = JSON.stringify(data)
}


function add(value) {
    const tableElement = document.getElementById(`${value}${ID_POSFIXES.table}`);
    const addFormElement = document.getElementById(`${value}${ID_POSFIXES.add}`);

    addClass(tableElement, ELEMENT_CLASSES.hidden);
    removeClass(addFormElement, ELEMENT_CLASSES.hidden);
}

function edit(value) {
    const tableElement = document.getElementById(`${value}${ID_POSFIXES.table}`);
    const editFormElement = document.getElementById(`${value}${ID_POSFIXES.edit}`);

    addClass(tableElement, ELEMENT_CLASSES.hidden);
    removeClass(editFormElement, ELEMENT_CLASSES.hidden);
}


function closeAllPanelsAndShowTable(value) {
    const tableElement = document.getElementById(`${value}${ID_POSFIXES.table}`);
    const addFormElement = document.getElementById(`${value}${ID_POSFIXES.add}`);
    const editFormElement = document.getElementById(`${value}${ID_POSFIXES.edit}`);

    addClass(addFormElement, ELEMENT_CLASSES.hidden);
    addClass(editFormElement, ELEMENT_CLASSES.hidden);
    removeClass(tableElement, ELEMENT_CLASSES.hidden);
}

// Helper functions

// Query param handling

function readCurrentTabFromQueryParams() {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(TAB_QUERY_PARAM);

}

function setQueryParams(key, value) {
    const searchParams = new URLSearchParams(window.location.search);
    searchParams.set(key, value);
    const newRelativePathQuery = window.location.pathname + "?" + searchParams.toString();
    history.pushState(null, "", newRelativePathQuery);
}

// Dom manipulation
function resetInnerHtmlById(id) {
    document.getElementById(id).innerHTML = "";
}

function setElelmentVisibilityById(id, value) {
    document.getElementById(id).style.display = value;
}

function setElementValueById(id, value) {
    document.getElementById(id).value = value;
}

function getElementValueById(id) {
    return document.getElementById(id).value
}

function addClass(element, className) {
    if (!element.classList.contains(className)) {
        element.classList.add(className);
    }
}

function removeClass(element, className) {
    if (element.classList.contains(className)) {
        element.classList.remove(className);
    }
}


// XHR

async function xhrGet(url) {
    return await fetch(`http://localhost:55475/${url}`, {
        method: XHR_TYPES.get,
        headers: DEFAULT_HEADERS
    })
}

async function xhrPost(url, request) {
    fetch(`http://localhost:55475/${url}`, {
        method: XHR_TYPES.post,
        headers: DEFAULT_HEADERS,
        body: JSON.stringify(request)
    })
}

async function xhrPut(url, request) {
    fetch(`http://localhost:55475/${url}`, {
        method: XHR_TYPES.put,
        headers: DEFAULT_HEADERS,
        body: JSON.stringify(request)
    })
}



async function xhrDelete(url, id) {
    return await fetch(`http://localhost:55475/${url}/${id}`, {
        method: XHR_TYPES.delete,
        headers: DEFAULT_HEADERS
    })
}
