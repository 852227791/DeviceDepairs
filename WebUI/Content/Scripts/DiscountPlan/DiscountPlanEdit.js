


bindItemNum = function () {
    $("#gridItemNum").datagrid({
        striped: true,//斑马线
        rownumbers: false,//行号
        singleSelect: true,//只允许选择一行
        pagination: false,//分页
        pageSize: 20,
        url: "../Item/GetItemNum",
        queryParams: { ItemID: ItemID },
        columns: [[
            { field: "ItemID", checkbox: false, hidden: true },
            { field: "Name", title: "缴费次数", width: 120, sortable: true }
        ]], sortName: "ItemID", sortOrder: "asc",
        onSelect: function (rowIndex, rowData) {
            if (rowData != undefined) {
                binditemDetail(rowData.ItemID);
            }

        }
    });
};
bindItemNum();
binditemDetail = function (itemId) {
    $("#gridItemDetail").edatagrid({
        striped: true,//斑马线
        rownumbers: false,//行号
        singleSelect: true,//只允许选择一行
        pagination: false,//分页
        pageSize: 20,
        url: "../ItemDetail/GetItemDetailByItem",
        queryParams: { ItemID: itemId },
        columns: [[
            { field: "ItemDetailID", checkbox: false, hidden: true },
            { field: "Name", title: "收费类别", width: 120, sortable: true },
            { field: "ShouldMoney", title: "应收金额", width: 80, sortable: true },
            { field: "DiscountMoney", title: "优惠金额", width: 80, sortable: false, editor: { type: 'numberspinner', options: { required: true, precision: 2, min: 0 } } }

        ]], sortName: "ItemDetailID", sortOrder: "asc"
    });
}


bindDiscountDetail = function () {
    $("#gridDiscountDetail").datagrid({
        striped: true,//斑马线
        rownumbers: false,//行号
        singleSelect: false,//只允许选择一行
        pagination: false,//分页
        pageSize: 20,
        url: "",
        queryParams: { ItemID: itemId },
        columns: [[
            { field: "ItemDetailID", checkbox: true },
            { field: "ItemID", checkbox: false, hidden: true },
            { field: "DiscountDetailID", checkbox: false, hidden: true },
            { field: "NumName", title: "缴费次数", width: 90, sortable: true },
            { field: "Name", title: "收费类别", width: 110, sortable: true },
            { field: "ShouldMoney", title: "应收金额", width: 80, sortable: true },
            { field: "DiscountMoney", title: "优惠金额", width: 80, sortable: false }

        ]], sortName: "NumName", sortOrder: "asc",
        toolbar: [{
            iconCls: 'icon-add',
            text: '添加',
            handler: function () {
                var numItemRow = $("#gridItemNum").datagrid("getSelected");
                if (numItemRow === null) {
                    Easy.centerShow("系统消息", "请选择缴费次数！", 2000);
                    return false;
                }

                $('#gridItemDetail').datagrid("acceptChanges");
                var detailRow = $("#gridItemDetail").edatagrid("getSelected");
                if (detailRow === null) {
                    Easy.centerShow("系统消息", "请选择缴费明细！", 2000);
                    return false;
                }

                if (parseFloat(detailRow.DiscountMoney) <= 0) {
                    Easy.centerShow("系统消息", "优惠金额不能小于等于0！", 2000);
                    return false;
                }
                if (parseFloat(detailRow.DiscountMoney) > parseFloat(detailRow.ShouldMoney)) {
                    Easy.centerShow("系统消息", "优惠金额不能大于该收费类别的应收金额！", 2000);
                    return false;
                }

                var disountData = $("#gridDiscountDetail").datagrid("getData");//获取所有已添加的优惠明细
                var flag = false;
                for (var i = 0; i < disountData.rows.length; i++) {
                    if (disountData.rows[i].ItemID === numItemRow.ItemID && disountData.rows[i].ItemDetailID === detailRow.ItemDetailID) {
                        flag = true;
                        break;
                    }
                }
                if (flag) {
                    Easy.centerShow("系统消息", "该收费明细已经优惠，不能重复添加！", 2000);
                    return false;
                }

                $('#gridDiscountDetail').datagrid('appendRow', {
                    ItemDetailID: detailRow.ItemDetailID,
                    NumName: numItemRow.Name,
                    ItemID: numItemRow.ItemID,
                    Name: detailRow.Name,
                    ShouldMoney: detailRow.ShouldMoney,
                    DiscountMoney: detailRow.DiscountMoney
                });
            }
        }, '-', {
            iconCls: 'icon-no',
            text: '删除',
            handler: function () {
                var rows = $("#gridDiscountDetail").datagrid("getSelections");
                if (rows.length === 0) {
                    Easy.centerShow("系统消息", "请选择优惠明细！", 2000);
                    return false;
                }
                $.messager.confirm('提示', '您确认想要删除吗？', function (r) {
                    if (r) {
                        var idString = "";
                        for (var i = 0; i < rows.length; i++) {
                            if (rows[i].DiscountDetailID != null && rows[i].DiscountDetailID != "" && rows[i].DiscountDetailID != undefined) {
                                idString += rows[i].DiscountDetailID + ",";
                            }
                            var index = $("#gridDiscountDetail").datagrid("getRowIndex", rows[i]);
                            $("#gridDiscountDetail").datagrid("deleteRow", index);
                        }
                        if (idString != "") {
                            idString = idString.substring(0, idString.length - 1);
                            var result = Easy.bindSelectInfo("../DiscountPlan/GetDeleteDiscountDetail", idString);
                            Easy.centerShow("系统消息", result.Message, 2000);
                        }
                    }
                });
            }
        }]

    });

}
bindDiscountDetail();


bindEditMethod = function () {
    if (DiscountPlanID != "0") {
        setTimeout(function () {
           
            var result1 = Easy.bindSelectInfo("../DiscountPlan/GetDiscountPlan", DiscountPlanID);

            var data = JSON.parse(result1.Data);
            $("#DiscountName").textbox("setValue", data.Name); 
            $("#DiscountPlanID").textbox("setValue", data.DiscountPlanID);
            var result2 = Easy.bindSelectInfo("../DiscountPlan/GetDicountDetail",DiscountPlanID);
            $("#gridDiscountDetail").datagrid("loadData", JSON.parse(result2.Data));
            $("#DiscountName").textbox("validate");

        }, 1);
    }
}
bindEditMethod();
setTimeout(function () { $("#gridItemNum").datagrid("selectRow", 0); }, 1);
bindValidte = function () {
    $.extend($.fn.validatebox.defaults.rules, {
        DiscountPlanName: {
            validator: function (value) {
                var discountPlanId = $("#DiscountPlanID").textbox("getValue");
                var result = Easy.bindSelectInfomation("../DiscountPlan/ValidatePlanName", { DiscountPlanID: discountPlanId, Name: value, ItemID: ItemID });
                if (result.IsError === false) {
                    return false;
                }
                else {
                    return true;
                }
            },
            message: '您输入的用优惠方案名称已存在！'
        }
    });
}
setTimeout(function () { bindValidte(); }, 1);