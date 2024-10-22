var trs = document.querySelectorAll('tr');
for (var i = 1; i < trs.length; i++) {
    var tds = trs[i].querySelectorAll('td');
    for (var j = 0; j < tds.length - 1; j++) {
        if (tds[j].textContent === 'No') {
            trs[i].style.backgroundColor = 'rgb(244,79,90)';

        } else if (tds[j].textContent === 'Yes') {
            trs[i].style.backgroundColor = '#53de9e';
        }
        tds[j].style.color = 'black';
        tds[j].style.fontWeight = 400;

    }
}