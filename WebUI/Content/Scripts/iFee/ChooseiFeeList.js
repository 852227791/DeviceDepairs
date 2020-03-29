var ifee = null;

updateGrid1Rows = function (index, offset) {
    ifee.datagrid('updateRow', {
        index: index,
        row: {
            OffSetMoney: parseFloat(offset).toFixed("2")
        }
    });
}

updateGrid1RowsCanMoney = function (index, canmoney) {
    ifee.datagrid('updateRow', {
        index: index,
        row: {
            CanMoney: parseFloat(canmoney).toFixed("2")
        }
    });
}

//查询条件数据
rerurnQueryData = function () {
    var queryData = {//得到用户输入的参数
        MenuID: menuId,
        DeptID: DeptID,
        txtStudentNameChooseFee: $("#txtStudentNameChooseFee").textbox("getValue"),
        txtVoucherChooseFee: $("#txtVoucherChooseFee").textbox("getValue")
    }
    return queryData;
}

//绑定表单1数据
initChooseiFeeGrid1 = function (queryData) {
    ifee = $("#choosefeegrid1").edatagrid({
        striped: true,//斑马线
        rownumbers: false,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        pageSize: 20,
        queryParams: queryData,//异步查询的参数
        columns: [[
            { field: "iFeeID", hidden: true },
            { field: "VoucherNum", title: "凭证号", width: 110, sortable: true },
            { field: "Dept", title: "收费单位", width: 120, sortable: true },
            { field: "Name", title: "人员姓名", width: 60, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "NoteNum", title: "票据号", width: 60, sortable: true },
            { field: "FeeContent", title: "收费项目", width: 60, sortable: true },
            { field: "FeeTime", title: "收费时间", width: 80, sortable: true },
            { field: "CanMoney", title: "可充抵金额", width: 70, sortable: true, align: 'right' },
            { field: "OffSetMoney", title: "充抵金额", width: 70, sortable: false, editor: { type: 'numberspinner', options: { precision: 2, min: 0 } } }
        ]],
        url: "../iFee/GetChooseiFeeList", sortName: "FeeTime", sortOrder: "desc",
        onLoadSuccess: function (data) {
            var choosedData2 = $('#choosefeegrid2').datagrid("getData");
            if (choosedData2.rows.length > 0) {
                for (var i = 0; i < data.rows.length; i++) {
                    for (var j = 0; j < choosedData2.rows.length; j++) {
                        if (data.rows[i]["iFeeID"] == choosedData2.rows[j]["iFeeID"]) {
                            var row = data.rows[i];
                            var index = ifee.datagrid('getRowIndex', row);
                            updateGrid1RowsCanMoney(index, parseFloat(data.rows[i].CanMoney) - parseFloat(choosedData2.rows[j]["OffSetMoney"]));
                        }
                    }
                }
            }
        }
    });
}

//绑定表单2数据
initChooseiFeeGrid2 = function () {
    $("#choosefeegrid2").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: false,//分页
        pageSize: 20,
        columns: [[
            { field: "iFeeID", hidden: true },
            { field: "VoucherNum", title: "凭证号", width: 110, sortable: true },
            { field: "Dept", title: "收费单位", width: 120, sortable: true },
            { field: "Name", title: "人员姓名", width: 60, sortable: true },
            { field: "IDCard", title: "身份证号", width: 140, sortable: true },
            { field: "NoteNum", title: "票据号", width: 60, sortable: true },
            { field: "FeeContent", title: "收费项目", width: 60, sortable: true },
            { field: "FeeTime", title: "收费时间", width: 80, sortable: true },
            { field: "OffSetMoney", title: "充抵金额", width: 80, sortable: true }
        ]]
    });
    var tempData = $("#editChooseOffSet").textbox("getValue");
    if (tempData != null && tempData != "") {
        $("#choosefeegrid2").datagrid("loadData", JSON.parse(tempData));
    }
}

//绑定搜索按钮的点击事件
bindChooseSearchClickEvent = function () {
    $("#btnSearchChooseFee").click(function () {//按条件进行查询数据，首先我们得到数据的值
        var queryData = rerurnQueryData();
        initChooseiFeeGrid1(queryData);//将值传递给
    });
    $("#btnResetChooseFee").click(function () {
        $("#fSearchChooseFee").form("reset");//重置表单
        initChooseiFeeGrid1({ MenuID: menuId, DeptID: DeptID });
    });
}

//添加事件
Add = function () {
    $("#btnAdd").click(function () {
        ifee.datagrid('acceptChanges');
        var row = ifee.datagrid("getSelected");//获取选中行
        if (row != null) {
            if (row.OffSetMoney == undefined || row.OffSetMoney == "") {
                Easy.centerShow("系统消息", "充抵金额不能为空", 1000);
                return false;
            }
            if (parseFloat(row.OffSetMoney) == 0) {
                Easy.centerShow("系统消息", "充抵金额不能为0", 1000);
                return false;
            }
            if (parseFloat(row.OffSetMoney) > parseFloat(row.CanMoney)) {
                Easy.centerShow("系统消息", "充抵金额不能大于可充抵金额", 1000);
                return false;
            }
            var data = $('#choosefeegrid2').datagrid("getData");
            for (var i = 0; i < data.rows.length; i++) {
                if (row.iFeeID == data.rows[i]["iFeeID"]) {
                    Easy.centerShow("系统消息", "同一收费项不能充抵两次", 1000);
                    return false;
                }
            }
            $('#choosefeegrid2').datagrid('appendRow', {
                iFeeID: row.iFeeID,
                VoucherNum: row.VoucherNum,
                Dept: row.Dept,
                Name: row.Name,
                IDCard: row.IDCard,
                NoteNum: row.NoteNum,
                FeeContent: row.FeeContent,
                FeeTime: row.FeeTime,
                OffSetMoney: row.OffSetMoney
            });
            //将充抵金额改为0
            var index = ifee.datagrid('getRowIndex', row);
            updateGrid1RowsCanMoney(index, parseFloat(row.CanMoney) - parseFloat(row.OffSetMoney));
            updateGrid1Rows(index, 0);
        }
        else {
            Easy.centerShow("系统消息", "请选择收费信息", 1000);
        }
    });
}

//移除事件
Remove = function () {
    $("#btnRemove").click(function () {
        var row = $('#choosefeegrid2').datagrid("getSelected");
        if (row != null) {
            var index2 = $('#choosefeegrid2').datagrid('getRowIndex', row);
            var data = ifee.datagrid("getData");
            for (var i = 0; i < data.rows.length; i++) {
                var newrow = data.rows[i];
                var index = ifee.datagrid('getRowIndex', newrow);
                if (newrow.iFeeID == row.iFeeID) {
                    updateGrid1RowsCanMoney(index, parseFloat(newrow.CanMoney) + parseFloat(row.OffSetMoney));
                }
            }
            $('#choosefeegrid2').datagrid('deleteRow', index2);
        } else {
            Easy.centerShow("系统消息", "请选择已冲抵信息", 1000);
        }
    });
}

//加载表单2数据
initChooseiFeeGrid2();

//加载表单1数据
initChooseiFeeGrid1({ MenuID: menuId, DeptID: DeptID });

//加载搜索按钮的点击事件
bindChooseSearchClickEvent();

//添加
Add();

//移除
Remove();
