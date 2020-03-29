bindForm = function () {
    $("#sEnrollTime").datebox({
        value: Easy.GetDateTimeNow(),
        editable: false,
        readonly: true
    });
    setTimeout(function () {
        $("#Id").textbox("setValue", sEnrollsProfessionID);
        $("#stuId").textbox("setValue", stuId);
        $("#nameTurn").textbox("setValue", name);
        $("#idCardTurn").textbox("setValue", idCard);
        //$("#sEnrollTime").datebox("setValue", sEnrollTime);
    }, 1);

};
bindForm();