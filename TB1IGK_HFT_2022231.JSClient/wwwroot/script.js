let competitors = [];
let categories = [];
let competitions = [];
let connection = null;
getdata();
getdata_category();
getdata_competition();
setupSignalR();
let IdToUpdate = -1;
let IdToUpdateCategory = -1;
let IdToUpdateCompetition = -1;

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

async function getdata() {
    await fetch('http://localhost:55475/competitor')
        .then(x => x.json())
        .then(y => {
            competitors = y;
            display();
        });
}

async function getdata_category() {
    await fetch('http://localhost:55475/category')
        .then(x => x.json())
        .then(y => {
            categories = y;
            display_category();
        });
}

async function getdata_competition() {
    await fetch('http://localhost:55475/competition')
        .then(x => x.json())
        .then(y => {
            competitions = y;
            display_competition();
        });
}


function display() { 
    document.getElementById('resultarea').innerHTML = "";
    competitors.forEach(t => {
        document.getElementById('resultarea').innerHTML +=
            "<tr><td>" + t.id + "</td><td>"
            + t.name + "</td><td>"
            + t.age + "</td><td>"
            + t.competitonID + "</td><td>"
            + t.categoryID + "</td><td>"
            + t.nation + "</td><td>" +
            `<button type="button" onclick="remove(${t.id})">Delete</button>` +
            `<button type="button" onclick="showupdate(${t.id})">Update</button>`
            + "</td></tr>";
    });
}

function display_category() {
    document.getElementById('categoryresultarea').innerHTML = "";
    categories.forEach(t => {
        document.getElementById('categoryresultarea').innerHTML +=
            "<tr><td>" + t.categoryNumber + "</td><td>"
            + t.ageGroup + "</td><td>"
            + t.boatCategory + "</td><td>" +
            `<button type="button" onclick="remove_category(${t.categoryNumber})">Delete</button>` +
            `<button type="button" onclick="showupdate_category(${t.categoryNumber})">Update</button>`
            + "</td></tr>";
    });
}

function display_competition() {
    document.getElementById('competitionresultarea').innerHTML = "";
    competitions.forEach(t => {
        document.getElementById('competitionresultarea').innerHTML +=
            "<tr><td>" + t.competitorID + "</td><td>"
            + t.opponentID + "</td><td>"
            + t.numberOfRacescAgainstEachOther + "</td><td>" 
            + t.location + "</td><td>"
            + t.distance + "</td><td>" +
        `<button type="button" onclick="remove_competition(${t.id})">Delete</button>` +
        `<button type="button" onclick="showupdate_competition(${t.id})">Update</button>`
            + "</td></tr>";
    });
}



function remove(id) {
    fetch('http://localhost:55475/competitor/' + id, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json', },
        body: null })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata();
        })
        .catch((error) => { console.error('Error:', error); });

}

function remove_category(categoryNumber) {
    fetch('http://localhost:55475/category/' + categoryNumber, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json', },
        body: null
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata_category();
        })
        .catch((error) => { console.error('Error:', error); });

}

function showupdate(id) {
    document.getElementById('nametoupdate').value = competitors.find(t => t['id'] == id)['name'];
    document.getElementById('agetoupdate').value = competitors.find(t => t['id'] == id)['age'];
    document.getElementById('competitonIDtoupdate').value = competitors.find(t => t['id'] == id)['competitonID'];
    document.getElementById('categoryIDtoupdate').value = competitors.find(t => t['id'] == id)['categoryID'];
    document.getElementById('nationtoupdate').value = competitors.find(t => t['id'] == id)['nation'];
    document.getElementById('updateformdiv').style.display = 'flex';
    IdToUpdate = id;
}

function update() {
    document.getElementById('updateformdiv').style.display = 'none';
    let name = document.getElementById('nametoupdate').value;
    let age = document.getElementById('agetoupdate').value;
    let competitonID = document.getElementById('competitonIDtoupdate').value;
    let categoryID = document.getElementById('categoryIDtoupdate').value;
    let nation = document.getElementById('nationtoupdate').value;
    fetch('http://localhost:55475/competitor', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            { name: name, id: IdToUpdate, age: age, competitonID: competitonID, categoryID: categoryID, nation: nation })
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata();
        })
        .catch((error) => { console.error('Error:', error); });
}

function showupdate_category(id) {
    document.getElementById('ageGrouptoupdate').value = categories.find(t => t['categoryNumber'] == id)['ageGroup'];
    document.getElementById('boatCategorytoupdate').value = categories.find(t => t['categoryNumber'] == id)['boatCategory'];
    document.getElementById('updateformdivcategory').style.display = 'flex';
    IdToUpdateCategory = id;
}

function update_category() {
    document.getElementById('updateformdivcategory').style.display = 'none';
    /*let categoryNumber = document.getElementById('categoryNumbertoupdate').value;*/
    let ageGroup = document.getElementById('ageGrouptoupdate').value;
    let boatCategory = document.getElementById('boatCategorytoupdate').value;
    fetch('http://localhost:55475/category', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            { ageGroup: ageGroup, categoryNumber: IdToUpdateCategory, boatCategory: boatCategory })
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata_category();
        })
        .catch((error) => { console.error('Error:', error); });
}

function showupdate_competition(id) {
    document.getElementById('competitorIDtoupdate').value = competitions.find(t => t['id'] == id)['competitorID'];
    document.getElementById('opponentIDtoupdate').value = competitions.find(t => t['id'] == id)['opponentID'];
    document.getElementById('numberOfRacesAgainstEachOthertoupdate').value = competitions.find(t => t['id'] == id)['numberOfRacesAgainstEachOther'];
    document.getElementById('locationtoupdate').value = competitions.find(t => t['id'] == id)['distance'];
    document.getElementById('updateformdivcompetition').style.display = 'flex';
    IdToUpdateCompetition = id;
}

function update_competition() {
    document.getElementById('updateformdivcompetition').style.display = 'none';
    let competitorID = document.getElementById('competitorIDtoupdate').value;
    let opponentID = document.getElementById('opponentIDtoupdate').value;
    let numberOfRacesAgainstEachOther = document.getElementById('numberOfRacesAgainstEachOthertoupdate').value;
    let location = document.getElementById('locationtoupdate').value;
    let distance = document.getElementById('distancetoupdate').value;
    fetch('http://localhost:55475/competition', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            { competitorID: competitorID, id: IdToUpdateCompetition, opponentID: opponentID, numberOfRacesAgainstEachOther: numberOfRacesAgainstEachOther, location: location, distance: distance })
    })
        .then(response => response)
        .then(data => {
            console.log('Success:', data);
            getdata_competition();
        })
        .catch((error) => { console.error('Error:', error); });
}



function create() {
    let name = document.getElementById('name').value;
    let age = document.getElementById('age').value;
    let competitonID = document.getElementById('competitonID').value;
    let categoryID = document.getElementById('categoryID').value;
    let nation = document.getElementById('nation').value;
    fetch('http://localhost:55475/competitor', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            { name: name, age:age, competitonID: competitonID, categoryID: categoryID, nation: nation })})
        .then(response => response)
        .then(data =>
        {
            console.log('Success:', data);
            getdata();
        })
        .catch((error) => { console.error('Error:', error); });
}

function create_category() {
    /*let categoryNumber = document.getElementById('categoryNumber').value;*/
    let ageGroup = document.getElementById('ageGroup').value;
    let boatCategory = document.getElementById('boatCategory').value;
    fetch('http://localhost:55475/category', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', },
        body: JSON.stringify(
            { /*categoryNumber: categoryNumber,*/ ageGroup: ageGroup, boatCategory: boatCategory })
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
