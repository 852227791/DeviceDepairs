bindFormEvent = function () {
    setTimeout(function () {
        $("#sOrderID").textbox("setValue", sOrderID);
        $("#ChangeShouldMoney").numberspinner("setValue", tempShoudMoney);
    }, 1);



};

bindFormEvent();