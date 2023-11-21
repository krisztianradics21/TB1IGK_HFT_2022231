fetch('http://localhost:55475/competitor')
    .then(x => x.json())
    .then(y => console.log(y));