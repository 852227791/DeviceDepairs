bindFormData = function () {
    var id = $("#deptTree").tree("getSelected").id;
    $("#editsDeptID").val(id);
}

//绑定验证事件
bindCheckEvent = function () {
    var data = { "jsontext": [] };
    var row1 = {};
    row1.url = "../sGive/CheckNameRepeat";
    row1.id = "#editsGiveID";
    row1.type = [{}];//row2.type = [{},{}];
    row1.type[0].typeId = "#editsDeptID";
    row1.type[0].type = "3";
    data.jsontext.push(row1);
    Easy.checkValue(data);
}

//绑定配品信息的方法
bindSelectGiveInfo = function (sGiveID) {
    if (sGiveID != "0") {
        setTimeout(function () {
            var result = Easy.bindSelectInfo("../sGive/SelectsGive", sGiveID);
            var detail = JSON.parse(result.Message)[0];
            $("#editsGiveID").textbox("setValue", detail.sGiveID);
            $("#editName").textbox("setValue", detail.Name);
            $("#editMoney").textbox("setValue", detail.Money);
            $("#editRemark").textbox("setValue", detail.Remark.replace(/<br \/>/g, "\r\n"));
        }, 1);
    }
}

bindFormData();

bindSelectGiveInfo(sGiveID);//绑定配品信息

//验证名称是否重复
bindCheckEvent();