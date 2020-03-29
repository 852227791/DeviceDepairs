bindCheckEvent = function () {
    var data = { "jsontext": [] };
    var row1 = {};
    row1.url = "../DeptArea/FormValidateDeptAreaName";
    row1.id = "#DeptAreaID";
    row1.type = [{}];//row2.type = [{},{}];
    row1.type[0].typeId = "#DeptID";
    row1.type[0].type = "3";
    data.jsontext.push(row1);
    Easy.checkValue(data);
}
bindFormEvemt = function () {
    setTimeout(function () {
        $("#DeptID").textbox("setValue", DeptID);
    }, 1);
}
bindSelectInfo = function () {
    if (deptAreaId != "0") {
        setTimeout(function () {
            var row = $("#grid").datagrid("getSelected");
            $("#DeptAreaID").textbox("setValue", row.DeptAreaID);
            $("#Name").textbox("setValue", row.Name);
            $("#Queue").numberspinner("setValue", row.Queue);
        }, 1);
    }
}

;
bindFormEvemt();

bindSelectInfo();
bindCheckEvent();