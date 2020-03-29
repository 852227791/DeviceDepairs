
initChooseStudentTable = function (queryData) {
    $("#choosestudentGrid").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,//异步查询的参数
        columns: [[
            { field: "StudentID", hidden: true },
            { field: "EnrollNum", title: "学号", width: 80, sortable: true },
            { field: "Name", title: "姓名", width: 80, sortable: true },
            { field: "IDCard", title: "身份证号", width: 150, sortable: true },
            { field: "Sex", title: "性别", width: 50, sortable: true },
            { field: "Mobile", title: "联系电话", width: 100, sortable: true },
            { field: "QQ", title: "QQ号", width: 100, sortable: true, },
            { field: "WeChat", title: "微信号", width: 100, sortable: true, },
            { field: "Address", title: "地址", width: 150, sortable: true, },
            { field: "Status", title: "状态", width: 60, sortable: true },
            { field: "DeptName", title: "校区", width: 120, sortable: true }
        ]],
        sortName: "StudentID", sortOrder: "desc"
    });
}

//绑定搜索按钮的点击事件
bindChooseStudentSearchClickEvent = function () {
    $("#btnSearchChooseStudent").click(function () {
        var name = $("#txtNameChooseStudent").textbox("getValue");
        var idCard = $("#txtIDCardChooseStudent").textbox("getValue");
        var enrollNum = $("#txtEnrollNum").textbox("getValue");
        if (name === "" && idCard === "" && enrollNum === "") {
            $("#choosestudentGrid").datagrid("loadData", []);
        }
        else {
            var queryData = {//得到用户输入的参数
                txtName: name,
                txtIDCard: idCard,
                txtEnrollNum: enrollNum
            }
            $("#choosestudentGrid").datagrid({
                url: "../Student/GetChooseStudentList",
                queryParams: queryData
            });
            //initChooseStudentTable(queryData);//将值传递给
        }
    });
    $("#btnResetChooseStudent").click(function () {
        $("#fSearchChooseStudent").form("reset");//重置表单
        $("#choosestudentGrid").datagrid("loadData", []);
    });
}
initChooseStudentTable({});
bindChooseStudentSearchClickEvent();

