//绑定表单数据
bindFormEvent = function () {
    $("#ParentID").combotree({
        url: "../Dept/GetDeptTree",
        queryParams: { MenuID: menuId, IsEdit: "Yes", Status: "1" },
        animate: true,
        lines: true
    });
}

//绑定验证事件
bindCheckEvent = function () {
    var data = { "jsontext": [] };
    var row1 = {};
    row1.url = "../Dept/CheckDeptName";
    row1.id = "#DeptID";
    row1.type = [{}];
    row1.type[0].typeId = "#ParentID";
    row1.type[0].type = "2";
    data.jsontext.push(row1);
    var row2 = {};
    row2.url = "../Dept/CheckDeptIDIsParentID";
    row2.id = "#DeptID";
    row2.type = [{}];
    row2.type[0].typeId = "#ParentID";
    row2.type[0].type = "2";
    data.jsontext.push(row2);
    var row3 = {};
    row3.url = "../Dept/CheckCode";
    row3.id = "#DeptID";
    row3.type = [];
    data.jsontext.push(row3);
    Easy.checkValue(data);
}

//绑定显示部门信息的方法
bindSelectDeptInfo = function (deptId) {
    if (deptId != "0") {
        setTimeout(function () {
            var result = Easy.bindSelectInfo("../Dept/SelectDept", deptId);
            var dept = JSON.parse(result.Message)[0];
            $("#DeptID").textbox("setValue", dept.DeptID);
            $("#Name").textbox("setValue", dept.Name);
            $("#ParentID").combotree("setValue", dept.ParentID);
            $("#ShortName").textbox("setValue", dept.ShortName);
            $("#Code").textbox("setValue", dept.Code);
            $("#Queue").textbox("setValue", dept.Queue);
            $("#Remark").textbox("setValue", dept.Remark.replace(/<br \/>/g, "\r\n"));
        }, 1);
    }
}

bindFormEvent();//加载表单数据

bindSelectDeptInfo(DeptID);//显示部门信息

//验证名称是否重复
bindCheckEvent();