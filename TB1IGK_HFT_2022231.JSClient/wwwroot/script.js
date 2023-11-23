let competitors = [];
let connection = null;
getdata();
setupSignalR();
let IdToUpdate = -1;

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
            //console.log(competitors);
            display();
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

function showupdate(id) {
    document.getElementById('nametoupdate').value = competitors.find(t => t['id'] == id)['name'];
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


