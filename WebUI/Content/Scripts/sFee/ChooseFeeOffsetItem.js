bindChooseOffset = function () {
    //学费
    $("#choosesFeeOffset").edatagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        queryParams: { MenuID: menuId },
        pageSize: 10,
        columns: [[
            { field: "ID", hidden: true },
            { field: "Type", hidden: true },
            { field: "VoucherNum", title: "凭证号", width: 120, sortable: true },
            { field: "StudName", title: "学生姓名", width: 60, sortable: true },
            { field: "IDCard", title: "身份证号", width: 135, sortable: true },
            { field: "CanMoney", title: "可充抵金额", width: 75, sortable: true, align: 'right', halign: 'left' },
            { field: "Money", title: "充抵", width: 80, sortable: true, editor: { type: 'numberspinner', options: { precision: 2, min: 0 } } },
            { field: "NoteNum", title: "票据号", width: 70, sortable: true },
            { field: "Content", title: "缴费内容", width: 170, sortable: true },
            { field: "FeeTime", title: "收费时间", width: 90, sortable: true },
            { field: "Dept", title: "收费单位", width: 80, sortable: true }
        ]],
        sortName: "ID", sortOrder: "desc",
        onLoadSuccess: function (data) {
            for (var j = 0; j < totalData.length; j++) {
                for (var i = 0; i < data.rows.length; i++) {
                    if (data.rows[i].ID === totalData[j].ID && data.rows[i].Type === totalData[j].Sort) {
                        var row = data.rows[i];
                        var index = $("#choosesFeeOffset").datagrid('getRowIndex', row);
                        var money = parseFloat(row.CanMoney) - (totalData[j].Money);
                        updateRows("#choosesFeeOffset", index, money, "0.00");
                    }
                }
            }
        }
    });
    //杂费
    $("#chooseiFeeOffset").edatagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        queryParams: { MenuID: menuId },
        pageSize: 10,
        columns: [[
            { field: "ID", hidden: true },
            { field: "Type", hidden: true },
            { field: "VoucherNum", title: "凭证号", width: 110, sortable: true },
            { field: "StudName", title: "学生姓名", width: 80, sortable: true },
            { field: "IDCard", title: "身份证号", width: 135, sortable: true },
            { field: "CanMoney", title: "可充抵金额", width: 75, sortable: true, align: 'right', halign: 'left' },
            { field: "Money", title: "充抵", width: 100, sortable: true, editor: { type: 'numberspinner', options: { precision: 2, min: 0 } } },
            { field: "NoteNum", title: "票据号", width: 90, sortable: true },
            { field: "Content", title: "缴费内容", width: 120, sortable: true }
        ]],
        sortName: "ID", sortOrder: "desc", onLoadSuccess: function (data) {
            for (var j = 0; j < totalData.length; j++) {
                for (var i = 0; i < data.rows.length; i++) {
                    if (data.rows[i].ID === totalData[j].ID && data.rows[i].Type === totalData[j].Sort) {
                        var row = data.rows[i];
                        var index = $("#chooseiFeeOffset").datagrid('getRowIndex', row);
                        var money = parseFloat(row.CanMoney) - (totalData[j].Money);
                        updateRows("#chooseiFeeOffset", index, money, "0.00");
                    }
                }
            }
        }
    });
    //证书
    $("#chooseFeeOffset").edatagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行
        pagination: true,//分页
        queryParams: { MenuID: menuId },
        pageSize: 10,
        columns: [[
            { field: "ID", hidden: true },
            { field: "Type", hidden: true },
            { field: "VoucherNum", title: "凭证号", width: 110, sortable: true },
            { field: "StudName", title: "学生姓名", width: 80, sortable: true },
            { field: "IDCard", title: "身份证号", width: 135, sortable: true },
            { field: "CanMoney", title: "可充抵金额", width: 75, sortable: true, align: 'right', halign: 'left' },
            { field: "Money", title: "充抵", width: 100, sortable: true, editor: { type: 'numberspinner', options: { precision: 2, min: 0 } } },
            { field: "NoteNum", title: "票据号", width: 90, sortable: true },
            { field: "Content", title: "缴费内容", width: 120, sortable: true }
        ]],
        sortName: "ID", sortOrder: "desc", onLoadSuccess: function (data) {
            for (var j = 0; j < totalData.length; j++) {
                for (var i = 0; i < data.rows.length; i++) {
                    if (data.rows[i].ID === totalData[j].ID && data.rows[i].Type === totalData[j].Sort) {
                        var row = data.rows[i];
                        var index = $("#chooseFeeOffset").datagrid('getRowIndex', row);
                        var money = parseFloat(row.CanMoney) - (totalData[j].Money);
                        updateRows("#chooseFeeOffset", index, money, "0.00");
                    }
                }
            }
        }
    });

}
bindChooseOffset();

bindOffsetList = function (data) {
    $("#sfeeOffset").datagrid({
        striped: true,//斑马线
        rownumbers: true,//行号
        singleSelect: true,//只允许选择一行sfeeOffset
        pagination: false,//分页
        showFooter: true,
        data: data,
        queryParams: { MenuID: menuId },
        columns: [[
            { field: "ID", hidden: true },
            {
                field: "Sort", title: "充抵来源", width: 100, sortable: true,
                formatter: function (value, row, index) {
                    if (value === "1")
                        return "学费";
                    else if (value === "2")
                        return "杂费";
                    else if (value === "3")
                        return "证书"
                }
            },
            { field: "VoucherNum", title: "凭证号", width: 110, sortable: true },
            { field: "StudName", title: "学生姓名", width: 80, sortable: true },
            { field: "IDCard", title: "身份证号", width: 135, sortable: true },
            { field: "NoteNum", title: "票据号", width: 90, sortable: true },
            { field: "Money", title: "充抵金额", width: 100, sortable: true }

        ]],
        sortName: "ID", sortOrder: "asc"
    });
}
bindOffsetList(allData);
//获取搜索表单
bindSearchFormMethod = function () {
    var queueString = {
        MenuID: menuId,
        name: $("#txtcName").val(),
        idCard: $("#txtcIDCard").val(),
        voucherNum: $("#txtcVoucher").val(),
        noteNum: $("#txtcNoteNum").val()
    };
    return queueString;
}
//绑定搜索按钮的点击事件
bindSearchClickEvent = function () {
    $("#btncSearch").click(function () {
        bindLoadData();
    });
    $("#btncReset").click(function () {
        $("#fcSearch").form("reset");//重置表单
        bindLoadData();
    });
}

$("#btnAddOffset").click(function () {
    var index = bindGetTabsIndexMethod();
    getSaveData(index);//保存行
    var row = getSelectRowMethod(index);
    if (row === null) {
        Easy.centerShow("系统消息", "请选择收费项信息", 2000);
        return false;
    }
    if (row.Money === "") {
        Easy.centerShow("系统消息", "充抵金额不能为空", 2000);
        return false;
    }
    var offsetData = $("#sfeeOffset").datagrid("getData");//获取已添加的充抵费用信息
    if (parseFloat(row.Money) <= 0) {
        Easy.centerShow("系统消息", "充抵金额不能小于或等于0", 2000);
        return false;
    }
    if (parseFloat(row.Money) > parseFloat(row.CanMoney)) {
        Easy.centerShow("系统消息", "可冲抵金额不足", 2000);
        return false;
    }
    var flag = false;
    for (var i = 0; i < offsetData.rows.length; i++) {
        if (row.ID === offsetData.rows[i].ID && row.Type === offsetData.rows[i].Sort) {
            flag = true;
        }
    }
    if (flag) {
        Easy.centerShow("系统消息", "同一个收费项不能同时充抵两次", 2000);
        return false;
    }

    $('#sfeeOffset').datagrid('appendRow', {
        ID: row.ID,
        Sort: row.Type,
        Money: row.Money,
        VoucherNum: row.VoucherNum,
        NoteNum: row.NoteNum,
        StudName: row.StudName,
        IDCard: row.IDCard,
        Content: row.Content,
        FeeTime: row.FeeTime
    });
    bindUpdateRowMethod(index, row, row.Money);//更新被充抵行


});

//获取选中行
getSelectRowMethod = function (index) {
    var grid = bindgetGridID(index);
    var row = $("" + grid + "").datagrid('getSelected');
    return row;
};
///保存editor内容
getSaveData = function (index) {
    var grid = bindgetGridID(index);
    $("" + grid + "").edatagrid("acceptChanges");
};

//绑定跟新行的方法
bindUpdateRowMethod = function (index, row, money) {
    var grid = bindgetGridID(index);
    var rowIndex = $("" + grid + "").datagrid("getRowIndex", row);//获取行号
    var tempMoney = (parseFloat(row.CanMoney) - parseFloat(money)).toFixed("2");
    $("" + grid + "").datagrid('updateRow', {
        index: rowIndex,
        row: {
            CanMoney: tempMoney,
            Money: '0.00'
        }
    });
};
//获取grid表格ID
bindgetGridID = function (index) {
    var grid = "";
    if (index === 0)
        grid = "#choosesFeeOffset";
    else if (index === 1)
        grid = "#chooseiFeeOffset";
    else if (index === 2)
        grid = "#chooseFeeOffset";
    return grid;
};
//移除
$("#btnRemoveOffset").click(function () {
    var index = bindGetTabsIndexMethod();
    var row = $("#sfeeOffset").datagrid("getSelected");
    if (row === null) {
        Easy.centerShow("系统消息", "请选择收费项信息", 2000);
        return false;
    }
    var rowIndex = $("#sfeeOffset").datagrid("getRowIndex", row);//获取行号
    var grid = bindgetGridID(index);
    var data = $("" + grid + "").edatagrid("getData");
    for (var i = 0; i < data.rows.length; i++) {
        if (row.ID === data.rows[i].ID && row.Sort === data.rows[i].Type) {
            var newRowIndex = $("" + grid + "").edatagrid("getRowIndex", data.rows[i]);
            var tempMoney = (parseFloat(data.rows[i].CanMoney) + parseFloat(row.Money)).toFixed("2");
            $("" + grid + "").datagrid('updateRow', {
                index: newRowIndex,
                row: {
                    CanMoney: tempMoney,
                    Money: '0.00'
                }
            });
        }
    }
    $('#sfeeOffset').datagrid('deleteRow', rowIndex);
});
///获取当前选中标签的index
bindGetTabsIndexMethod = function () {
    var tab = $('#chooseFeeofsetTas').tabs('getSelected');
    var index = $('#chooseFeeofsetTas').tabs('getTabIndex', tab);
    return index;
};
//返回url
getDataUrl = function (index) {
    var url;
    if (index === 0)
        url = "../OffsetChooser/GetsFeeList";
    else if (index === 1)
        url = "../OffsetChooser/GetiFeeList";
    else if (index === 2)
        url = "../OffsetChooser/GetFeeList";
    return url;
}

bindSearchClickEvent();
//加载列表数据
bindLoadData = function () {
    var queryData = bindSearchFormMethod();
    var index = bindGetTabsIndexMethod();
    var gird = bindgetGridID(index);
    var urlstr = getDataUrl(index);
    $(gird).datagrid({
        url: urlstr,
        queryParams: queryData
    });
}
//tabs点击事件绑定
bindloadList = function () {
    $('#chooseFeeofsetTas').tabs({
        selected: 0,
        onSelect: function (title, index) {
            var queryData = bindSearchFormMethod();
            var gird = bindgetGridID(index);
            var urlstr = getDataUrl(index);
            $(gird).datagrid({
                url: urlstr,
                queryParams: queryData
            });
        }
    });
}
bindloadList();



updateRows = function (gird, index, money, offset) {
    $("" + gird + "").datagrid('updateRow', {
        index: index,
        row: {
            CanMoney: parseFloat(money).toFixed("2"),
            Money: parseFloat(offset).toFixed("2")
        }
    });
}