let competitors = [];

fetch('http://localhost:55475/competitor')
    .then(x => x.json())
    .then(y => {
        competitors = y;
        console.log(competitors);
        display();
    });
function display() {
    competitors.forEach(t => {
        document.getElementById('resultarea').innerHTML +=
            "<tr><td>" + t.id + "</td><td>"
            + t.name + "</td><td>"
            + t.age + "</td><td>"
            + t.competitonID + "</td><td>"
            + t.categoryID + "</td><td>"
        + t.nation + "</td></tr>"
    });
}