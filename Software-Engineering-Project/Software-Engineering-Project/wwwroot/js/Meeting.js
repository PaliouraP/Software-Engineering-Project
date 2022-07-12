const date = new Date();


const renderCalendar = () => {
  date.setDate(1);

  const monthDays = document.querySelector(".days");

  const lastDay = new Date(
    date.getFullYear(),
    date.getMonth() + 1,
    0
  ).getDate();

  const prevLastDay = new Date(
    date.getFullYear(),
    date.getMonth(),
    0
  ).getDate();

  const firstDayIndex = date.getDay();

  const lastDayIndex = new Date(
    date.getFullYear(),
    date.getMonth() + 1,
    0
  ).getDay();

  const nextDays = 7 - lastDayIndex - 1;

  const months = [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December",
  ];

    document.querySelector(".date h1").innerHTML = months[date.getMonth()];

  document.querySelector(".date p").innerHTML = new Date().toDateString();


  let days = "";

  for (let x = firstDayIndex; x > 0; x--) {
      days += `<div class="prev-date">${prevLastDay - x + 1}</div>`;
      
  }

  for (let i = 1; i <= lastDay; i++) {
    if (
      i === new Date().getDate() &&
      date.getMonth() === new Date().getMonth()
    ) {
        days += `<div class="today" id="${i}" onclick="popup(this.id)">${i}</div>`;
    } else {
        days += `<div id="${i}" onclick="popup(this.id)">${i}</div>`;
      }
  }

  for (let j = 1; j <= nextDays; j++) {
    days += `<div class="next-date">${j}</div>`;
   }
    monthDays.innerHTML = days;
};

document.querySelector(".prev").addEventListener("click", () => {
  date.setMonth(date.getMonth() - 1);
  renderCalendar();
});

document.querySelector(".next").addEventListener("click", () => {
  date.setMonth(date.getMonth() + 1);
  renderCalendar();
});




renderCalendar();

function dateFunction(day_id) {
    var month = document.getElementById("month");
    //if (day_id == 1 || day_id == 21 || day_id == 31) {
    document.getElementById("selected_day").innerText = day_id; // + "st of " + month.innerText;
    document.getElementById("selected_month").innerText = month.innerText;
    /*} else if (day_id == 2 || day_id == 22) {
        document.getElementById("selected_date").innerText = day_id + "nd of " + month.innerText;
    } else if (day_id == 3 || day_id == 23) {
        document.getElementById("selected_date").innerText = day_id + "rd of " + month.innerText;
    } else {
        document.getElementById("selected_date").innerText = day_id + "th of " + month.innerText;
    }*/
}

function popup(clicked_id) {
    var dialog = document.getElementById("dialogtext");
    dialog.classList.toggle("show");
    dateFunction(clicked_id);
}

