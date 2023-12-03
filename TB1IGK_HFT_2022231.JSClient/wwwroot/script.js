const API_URLS = {
    competitor: "competitor",
    category: "category",
    competition: "competition",
    getCompetitorsByBoatCategory: "stat/CompetitorsByBoatCategory",
    getCompetitorWithAllRelevantData: "stat/CompetitorWithAllRelevantData",
    getOpponentsByName: "stat/OpponentsByName",
    getCompetitionBasedOnCompetitorsNameAndNation: "stat/Competition_BasedOnCompetitorsNameAndNation",
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

let competitiorIdToUpdate = -1;
let categoryIdToUpdate = -1;
let competitionIdToUpdate = -1;

/**
 * [ HANDLE TABS ]
 */

/**
 *  When the window loads, ead the query params, or set the default tab
 */

window.addEventListener('load', () => {
    // read the current tab from query param
    const currentTab = readCurrentTabFromQueryParams();

    // if the query param exists, open it
    if (currentTab && !!TAB_UIDS[currentTab]) {
        openTab(document.getElementById(`${currentTab}Tab`), currentTab);
        return;
    }

    openTab(document.getElementById('competitorsTab'), TAB_UIDS.competitors);
})

/**
 * Open the given tab
 * @param {HtmlButtonElement} evt the div element on the tabs
 * @param {TAB_UIDS} tabId the tab to open
 * @returns
 */

function openTab(evt, tabId) {

    let tabcontent, tablinks;

    // get all tab contents, and set them to display.none
    tabcontent = document.getElementsByClassName("tabcontent");
    for (let i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = DISPLAY_VALUES.none;
    }

    // get all tab links, and set them to display.none
    tablinks = document.getElementsByClassName("tablinks");
    for (let i = 0; i < tablinks.length; i++) {
        removeClass(tablinks[i], ELEMENT_CLASSES.active);
    }

    // display the selected tab
    setElelmentVisibilityById(tabId, DISPLAY_VALUES.block);

    // update the query params to the given tab
    setQueryParams(TAB_QUERY_PARAM, tabId);

    // if the evt input has a currentTarget (only exists if the fuction was called from the index.html)
    if (evt.currentTarget) {
        // display it
        addClass(evt.currentTarget, ELEMENT_CLASSES.active);
    } else {
        // else call the same fuction for the evt input (inly if this function is called from the js)
        addClass(evt, ELEMENT_CLASSES.active);
    }

    // return if the tab is the other details tab
    if (tabId == TAB_UIDS.otherData) {
        return;
    }

    // else generate the display for the sleected tab
    switch (tabId) {
        case TAB_UIDS.competitors:
            getCompetitorsTableData();
            break
        case TAB_UIDS.categories:
            getCategoriesTableData();
            break;
        case TAB_UIDS.competitions:
            getCompetitionsTableData();
            break;
    }

    // if add or edit is open, close it
    closeAllPanelsAndShowTable(tabId);

}

/**
 * Sets up the signal for the CRUD methods
 */
function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:55475/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("CompetitorCreated", (user, message) => {
        getCompetitorsTableData();
    });

    connection.on("CompetitorDeleted", (user, message) => {
        getCompetitorsTableData();
    });

    connection.on("CompetitorUpdated", (user, message) => {
        getCompetitorsTableData();
    });
    connection.on("CategoryCreated", (user, message) => {
        getCategoriesTableData();
    });

    connection.on("CategoryDeleted", (user, message) => {
        getCategoriesTableData();
    });

    connection.on("CategoryUpdated", (user, message) => {
        getCategoriesTableData();
    });
    connection.on("CompetitionCreated", (user, message) => {
        getCompetitorsTableData();
    });

    connection.on("CompetitionDeleted", (user, message) => {
        getCompetitorsTableData();
    });

    connection.on("CompetitionUpdated", (user, message) => {
        getCompetitorsTableData();
    });

    connection.onclose(async () => {
        await start();
    });
    start();
}

/**
 * Waits for the conntection to start
 * Retries after 5 seconds
 */
async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

/**
 *   [ DISPLAY THE DATA ]
 */

/**
 * Gets the data for the competitors table
 */
function getCompetitorsTableData() {

    fetchGet(API_URLS.competitor)
        .then(response => response.json())
        .then(data => {
            competitors = data
            displayCompetitorsTableData();
        })

}

/**
 * Gets the data for the categories table
 */
async function getCategoriesTableData() {

    fetchGet(API_URLS.category)
        .then(response => response.json())
        .then(data => {
            categories = data
            displayCategoriesTableData();
        })
}

/**
 * Gets the data for the competitions table
 */
async function getCompetitionsTableData() {

    fetchGet(API_URLS.competition)
        .then(response => response.json())
        .then(data => {
            competitions = data
            displayCompetitionsTableData();
        })
}

/**
 *  [ DISPLAY DATA ]
 */

/**
 * Display the data for the competitors table
 */
function displayCompetitorsTableData() {
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
            `<button type="button" onclick="removeCompetitor(${competitior.id})">Delete</button>` +
            `<button type="button" onclick="showCompetitorUpdateForm(${competitior.id})">Update</button>`
            + "</td></tr>";
    });
}

/**
 * Display the data for the categories table
 */
function displayCategoriesTableData() {
    closeAllPanelsAndShowTable('categories');
    resetInnerHtmlById('categoryresultarea');
    categories.forEach(category => {
        document.getElementById('categoryresultarea').innerHTML +=
            "<tr><td>" + category.categoryNumber + "</td><td>"
        + category.ageGroup + "</td><td>"
        + category.boatCategory + "</td><td>" +
        `<button type="button" onclick="removeCategory(${category.categoryNumber})">Delete</button>` +
        `<button type="button" onclick="showCategoryUpdateForm(${category.categoryNumber})">Update</button>`
            + "</td></tr>";
    });
}

/**
 * Display the data for the competitions table
 */
function displayCompetitionsTableData() {
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
        `<button type="button" onclick="removeCompetition(${competition.id})">Delete</button>` +
        `<button type="button" onclick="showCompetitionUpdateForm(${competition.id})">Update</button>`
            + "</td></tr>";
    });
}


/**
 *  [ REMOVE DATA ]
 */

/**
 * Removes the competitior with the provided id, and refreshes the table
 * @param {any} id
 */
function removeCompetitor(id) {
    remove(API_URLS.competitor, id, getCompetitorsTableData);
}

/**
 * Removes the category with the provided id, and refreshes the table
 * @param {any} id
 */
function removeCategory(categoryNumber) {
    remove(API_URLS.category, categoryNumber, getCategoriesTableData);
}

/**
 * Removes the competition with the provided id, and refreshes the table
 * @param {any} id
 */
function removeCompetition(id) {
    remove(API_URLS.competition, id, getCompetitionsTableData);
}

/**
 * Calles the given URL with the given ID, then on success calles the provided successFn
 * @param {any} url
 * @param {any} id
 * @param {any} successFn
 */
function remove(url, id, successFn) {
    fetchDelete(url, id)
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            successFn();
        })
        .catch((error) => { console.error('Error:', error); });
}

/**
 *  [ UPDATE THE DATA ]
 */

/**
 * Display the competitor update form for the given id
 * @param {any} id
 */
function showCompetitorUpdateForm(id) {

    const currentCompetitor = competitors.find(t => t.id == id);

    setElementValueById('nameToUpdate', currentCompetitor.name);
    setElementValueById('agetoupdate', currentCompetitor.age);
    setElementValueById('competitonIDtoupdate', currentCompetitor.competitonID);
    setElementValueById('categoryIDtoupdate', currentCompetitor.categoryID);
    setElementValueById('nationtoupdate', currentCompetitor.nation);

    edit(TAB_UIDS.competitors);

    competitiorIdToUpdate = id;
}

/**
 * Updated the selected competitor
 */
function updateCompetitor() {
    closeAllPanelsAndShowTable(TAB_UIDS.competitors);

    const request = {
        id: competitiorIdToUpdate,
        name: getElementValueById('nameToUpdate'),
        age: getElementValueById('agetoupdate'),
        competitonID: getElementValueById('competitonIDtoupdate'),
        categoryID: getElementValueById('categoryIDtoupdate'),
        nation: getElementValueById('nationtoupdate')
    };

    update(API_URLS.competitor, request, getCompetitorsTableData);
}

/**
 * Display the category update form for the given id
 * @param {any} id
 */
function showCategoryUpdateForm(id) {

    const category = categories.find(t => t.categoryNumber == id)

    setElementValueById('ageGrouptoupdate', category.ageGroup);
    setElementValueById('boatCategorytoupdate', category.boatCategory);

    edit(TAB_UIDS.categories);

    categoryIdToUpdate = id;
}

/**
 * Updated the selected category
 */
function updateCategory() {
    closeAllPanelsAndShowTable(TAB_UIDS.categories);

    const request = {
        categoryNumber: categoryIdToUpdate,
        ageGroup: getElementValueById('ageGrouptoupdate'),
        boatCategory: getElementValueById('boatCategorytoupdate')
    }

    update(API_URLS.category, request, getCategoriesTableData);

}

/**
 * Display the competition update form for the given id
 * @param {any} id
 */
function showCompetitionUpdateForm(id) {

    const competition = competitions.find(t => t.id == id)

    setElementValueById('competitorIDtoupdate', competition.competitorID);
    setElementValueById('opponentIDtoupdate', competition.opponentID);
    setElementValueById('numberOfRacesAgainstEachOthertoupdate', competition.numberOfRacesAgainstEachOther);
    setElementValueById('locationtoupdate', competition.location);
    setElementValueById('distancetoupdate', competition.distance);

    edit(TAB_UIDS.competitions);


    competitionIdToUpdate = id;
}

/**
 * Updated the selected competition
 */
function updateCompetition() {
    
    closeAllPanelsAndShowTable(TAB_UIDS.competitions);

    const request = {
        competitorID: getElementValueById('competitorIDtoupdate'),
        id: competitionIdToUpdate,
        opponentID: getElementValueById('opponentIDtoupdate'),
        numberOfRacesAgainstEachOther: getElementValueById('numberOfRacesAgainstEachOthertoupdate'),
        location: getElementValueById('locationtoupdate'),
        distance: getElementValueById('distancetoupdate'),
    }

    update(API_URLS.competition, request, getCompetitionsTableData);

}

/**
 * Calles the given URL with the given ID, then on success calles the provided successFn
 * @param {any} url
 * @param {any} id
 * @param {any} successFn
 */
function update(url, request, successFn) {
    fetchPut(url, request)
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            successFn();
        })
        .catch((error) => { console.error('Error:', error); });
}

/**
 *  [ CREATE THE DATA ]
 */

/**
 * Creates a competitior
 */
function createCompetitor() {

    const request = {
        name: getElementValueById('name'),
        age: getElementValueById('age'),
        competitonID: getElementValueById('competitonID'),
        categoryID: getElementValueById('categoryID'),
        nation: getElementValueById('nation')
    }

    create(API_URLS.competitor, request, getCompetitorsTableData)

}

/**
 * Creates a category
 */
function createCategory() {

    const request = {
        ageGroup: getElementValueById('ageGroup'),
        boatCategory: getElementValueById('boatCategory')
    }

    create(API_URLS.category, request, getCategoriesTableData)
}

/**
 * Creates a competition
 */
function createCompetition() {

    const request = {
        competitorID: getElementValueById('competitorID'),
        opponentID: getElementValueById('opponentID'),
        numberOfRacesAgainstEachOther: getElementValueById('numberOfRacesAgainstEachOther'),
        location: getElementValueById('location'),
        distance: getElementValueById('distance')
    }

    create(API_URLS.competition, request, getCompetitionsTableData)

}

/**
 * Calles the provided url with the request object, then on success calles the provided successFn
 * 
 * @param {any} url
 * @param {any} request
 * @param {any} successFn
 */
function create(url, request, successFn) {
    fetchPost(url, request)
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            successFn();
        })
        .catch((error) => { console.error('Error:', error); });
}

/**
 *  [ NON CRUD METHODS ]
 */

/**
 * Gets the competitors by boat category
 */
function getCompetitorsByBoatCategory() {
    fetchGet(API_URLS.getCompetitorsByBoatCategory)
        .then(response => response.json())
        .then(data => {
            console.log(data)
            displayCompetitorsByBoatCategory(data)
        })
}

/**
 * Displays the competitors by boat category data
 * @param {any} data
 */
function displayCompetitorsByBoatCategory(data) {
    document.getElementById('competitorsbyboatcategory').innerHTML = JSON.stringify(data)
}

/**
 * Gets the competitors with all relevant data
 */
function getCompetitorWithAllRelevantData() {
    fetchGet(API_URLS.getCompetitorWithAllRelevantData)
        .then(response => response.json())
        .then(data => {
            console.log(data)
            displayCompetitionWithAllRelevantData(data)
        })
}
/**
 * Displays the competitors with all relevant data
 * @param {any} data
 */
function displayCompetitionWithAllRelevantData(data) {
    document.getElementById('competitorwithallrelevantdata').innerHTML = JSON.stringify(data)
}

/**
 * Gets all the opponents by name
 */
function getOpponentsByName() {
    fetchGet(API_URLS.getOpponentsByName)
        .then(response => response.json())
        .then(data => {
            console.log(data)
            displayOpponentsByName(data)
        })
}

/**
 * Displays all the oponents by name data
 * @param {any} data
 */
function displayOpponentsByName(data) {
    document.getElementById('OpponentsByName').innerHTML = JSON.stringify(data)
}

/**
 * Gets the competitions based on the competitors name and the nation
 */
function getCompetitionBasedOnCompetitorsNameAndNation() {
    fetchGet(API_URLS.getCompetitionBasedOnCompetitorsNameAndNation)
        .then(response => response.json())
        .then(data => {
            console.log(data)
            displayCompetition_BasedOnCompetitorsNameAndNation(data)
        })
}

/**
 * Displays the competitions based on the competitors name and the nation data
 * @param {any} data
 */
function displayCompetition_BasedOnCompetitorsNameAndNation(data) {
    document.getElementById('CompetitionBasedOnCompetitorsNameAndNation').innerHTML = JSON.stringify(data)
}

/**
 * Gets the avg age
 */
function getAvgAge() {
    fetchGet(API_URLS.getAvgAge)
        .then(response => response.json())
        .then(data => {
            console.log(data)
            displayAvgAge(data)
        })
}

/**
 * Displays the avg age data
 * @param {any} data
 */
function displayAvgAge(data) {
    document.getElementById('AvgAge').innerHTML = JSON.stringify(data)
}

/**
 *  [ CEATE AND UPDATE DOM HELPER FUNCTIONS ]
 */

/**
 * Hides the table element, and displays the add form for the given tab
 * @param {any} value
 */
function add(value) {
    // finds the table element
    const tableElement = document.getElementById(`${value}${ID_POSFIXES.table}`);
    // finds the add form element
    const addFormElement = document.getElementById(`${value}${ID_POSFIXES.add}`);

    // hides the table element
    addClass(tableElement, ELEMENT_CLASSES.hidden);
    // displays the add form element
    removeClass(addFormElement, ELEMENT_CLASSES.hidden);
}

/**
 * Hides the table element, and displays the edit form for the given tab
 * @param {any} value
 */
function edit(value) {
    // finds the table element
    const tableElement = document.getElementById(`${value}${ID_POSFIXES.table}`);
    // finds the edit form element
    const editFormElement = document.getElementById(`${value}${ID_POSFIXES.edit}`);

    // hides the table element
    addClass(tableElement, ELEMENT_CLASSES.hidden);
    // displays the edit form element
    removeClass(editFormElement, ELEMENT_CLASSES.hidden);
}

/**
 * Closes the add and edit form, and displays the table on the form
 * @param {any} value
 */
function closeAllPanelsAndShowTable(value) {
    const tableElement = document.getElementById(`${value}${ID_POSFIXES.table}`);
    const addFormElement = document.getElementById(`${value}${ID_POSFIXES.add}`);
    const editFormElement = document.getElementById(`${value}${ID_POSFIXES.edit}`);

    addClass(addFormElement, ELEMENT_CLASSES.hidden);
    addClass(editFormElement, ELEMENT_CLASSES.hidden);
    removeClass(tableElement, ELEMENT_CLASSES.hidden);
}

/**
 *  [ QUERY PARAMS ]
 */


/**
 * Reads the current tab value from the query params
 */
function readCurrentTabFromQueryParams() {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(TAB_QUERY_PARAM);

}

/**
 * Adds the received key value pair to the query params, and updates the state of the history
 * @param {any} key
 * @param {any} value
 */
function setQueryParams(key, value) {
    const searchParams = new URLSearchParams(window.location.search);
    searchParams.set(key, value);
    const newRelativePathQuery = window.location.pathname + "?" + searchParams.toString();
    history.pushState(null, "", newRelativePathQuery);
}

/**
 *  [ DOM MANIPILATION ]
 */

/**
 * Resets the innerHtml of the given element
 * @param {any} id
 */
function resetInnerHtmlById(id) {
    document.getElementById(id).innerHTML = "";
}

/**
 * Sets the display of the element to the received value
 * @param {any} id
 * @param {any} value
 */
function setElelmentVisibilityById(id, value) {
    document.getElementById(id).style.display = value;
}

/**
 * Sets the value of the element to the received value
 * @param {any} id
 * @param {any} value
 */
function setElementValueById(id, value) {
    document.getElementById(id).value = value;
}

/**
 * Gets the value of the element
 * @param {any} id
 * @returns
 */
function getElementValueById(id) {
    return document.getElementById(id).value
}

/**
 * Adds the class to the element, if it doesn't have it
 * @param {any} element
 * @param {any} className
 */
function addClass(element, className) {
    if (!element.classList.contains(className)) {
        element.classList.add(className);
    }
}

/**
 * Removes the class from the element, if it has it
 * @param {any} element
 * @param {any} className
 */
function removeClass(element, className) {
    if (element.classList.contains(className)) {
        element.classList.remove(className);
    }
}


/**
 *  [ FETCH ]
 */

async function fetchGet(url) {
    return await fetch(`http://localhost:55475/${url}`, {
        method: XHR_TYPES.get,
        headers: DEFAULT_HEADERS
    })
}

async function fetchPost(url, request) {
    fetch(`http://localhost:55475/${url}`, {
        method: XHR_TYPES.post,
        headers: DEFAULT_HEADERS,
        body: JSON.stringify(request)
    })
}

async function fetchPut(url, request) {
    fetch(`http://localhost:55475/${url}`, {
        method: XHR_TYPES.put,
        headers: DEFAULT_HEADERS,
        body: JSON.stringify(request)
    })
}


async function fetchDelete(url, id) {
    return await fetch(`http://localhost:55475/${url}/${id}`, {
        method: XHR_TYPES.delete,
        headers: DEFAULT_HEADERS
    })
}
