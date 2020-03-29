bindCheckEvent = function () {
    var data = { "jsontext": [] };
    var row1 = {};
    row1.url = "../Student/CheckIDCardIsRepeat";
    row1.id = "#idStudentID";
    row1.type = [];
    data.jsontext.push(row1);
    var row2 = {};
    row2.url = "../Student/CheckIDCardIsStandard";
    row2.id = "#idStudentID";
    row2.type = [];
    data.jsontext.push(row2);
    Easy.checkValue(data);
}

bindSelectInfo = function () {
    if (StudentID != "0") {
        setTimeout(function () {
            var result = Easy.bindSelectInfo("../Student/SelectStudent", StudentID);
            var p = JSON.parse(result.Message)[0];
            $("#idStudentID").textbox("setValue", p.StudentID);
            $("#idName").textbox("setValue", p.Name);
            //$("#idIDCard").textbox("setValue", p.IDCard);
        }, 1);
    }
}

bindCheckEvent();
bindSelectInfo();
