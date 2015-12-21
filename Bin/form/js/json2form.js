/*! JsonForm v1.1 http://www.ewikisoft.com/?utm_source=src Copyright 2014, wikisoft 本版本已授权，授权协议见本目录，请勿分发复制.
 *
 * Date: 2014年1月13日
 */
(function () {
    var JsonFormHelper = {addClass: function (ele, clsName) {
        if (!ele.className) {
            ele.className = clsName
        } else {
            if (ele.className.indexOf(clsName) == -1) {
                ele.className += " " + clsName
            }
        }
    }, onValueChane: function (input) {
        this.addClass(input, "json_form_dirty");
        this.validateInput(input)
    }, validateInput: function (input) {
        var v = input.value;
        if (v == "" && input.getAttribute("required") != null) {
            this.handlerEmpty(input);
            return false
        }
        if (v != "" && input.getAttribute("pattern") && !new RegExp("^" + input.getAttribute("pattern") + "$", "i").test(input.value)) {
            this.handlerInvalid(input);
            return false
        }
        return true
    }, handlerEmpty: function (input, conf) {
        var errMsg = conf ? ("字段[" + (conf.label) + "]不能为空") : input.title;
        if (errMsg) {
            alert(errMsg)
        }
        if (typeof input.focus == "function" || typeof input.focus == "object") {
            input.focus()
        }
    }, handlerInvalid: function (input, conf) {
        var errMsg = conf ? ("字段[" + (conf.label) + "]的值非法") : input.title;
        if (errMsg) {
            alert(errMsg)
        }
        if (typeof input.focus == "function" || typeof input.focus == "object") {
            input.focus()
        }
    }, getNextSibling: function (n) {
        var x = n.nextSibling;
        while (x && x.nodeType != 1) {
            x = x.nextSibling
        }
        return x
    }, showNextSibling: function (ele, visible) {
        var n = this.getNextSibling(ele);
        while (n) {
            var isHidden = n.getAttribute("hidden") != null;
            n.style.display = isHidden ? "none" : (visible ? "" : "none");
            n = this.getNextSibling(n)
        }
    }, isInArray: function (key, array) {
        var i = array.length;
        while (i--) {
            if (key == array[i]) {
                return true
            }
        }
        return false
    }, removeRow: function (row) {
        var tbBody = row.parentNode;
        if (tbBody.rows.length == 1 && !tbBody.tempRow) {
            row.cells[0].innerHTML = 0;
            tbBody.tempRow = row.cloneNode(true)
        }
        tbBody.removeChild(row)
    }, addRow: function (table) {
        var tbBody = table.tBodies[0];
        if (!tbBody) {
            return false
        }
        var lastRow = null;
        if (tbBody.rows.length == 0) {
            lastRow = tbBody.tempRow
        } else {
            lastRow = tbBody.rows[tbBody.rows.length - 1];
            if (!lastRow) {
                return false
            }
        }
        var newRow = lastRow.cloneNode(true);
        var labels = newRow.getElementsByTagName("label");
        var len = labels.length;
        while (len--) {
            var lb = labels[len];
            var forId = lb.htmlFor;
            var newId = "ele_json_new_" + len + "_" + (new Date().getTime() % 1000000);
            lb.htmlFor = newId;
            var ip = this.getNextSibling(lb);
            ip.readOnly = false;
            ip.id = newId;
            ip.value = ""
        }
        table.tBodies[0].appendChild(newRow);
        var rowNumber = newRow.cells[0].innerHTML - 0;
        newRow.cells[0].innerHTML = rowNumber + 1
    }, each: function (array, fn) {
        var len = array.length;
        for (var i = 0; i < len; i++) {
            fn(array[i], i)
        }
    }, getObjType: function (obj) {
        if (obj.constructor == Array) {
            return"Array"
        } else {
            if (obj.constructor == Object) {
                return"Object"
            } else {
                if (obj.constructor == Number) {
                    return"Number"
                } else {
                    if (obj.constructor == Boolean) {
                        return"Boolean"
                    } else {
                        if (obj.constructor == String) {
                            return"String"
                        } else {
                            return"unknow"
                        }
                    }
                }
            }
        }
    }};
    var FormElementFactory = {defaultDelimiter: ",", getLabelText: function (fieldConfig, name_or_index) {
        if (fieldConfig && fieldConfig.label) {
            return fieldConfig.label.toString()
        } else {
            return typeof name_or_index == "undefined" ? "" : name_or_index
        }
    }, generateDatalist: function (list, id) {
        var len = list.length;
        var html = [];
        html.push("<datalist id='" + id + "'>");
        var isTextValue = len > 0 && typeof list[0] == "object";
        for (var i = 0; i < len; i++) {
            var v = isTextValue ? list[i].value : list[i];
            html.push("<option value='" + v + "' />")
        }
        html.push("</datalist>");
        return html.join("")
    }, generateInputHtml: function (param) {
        param = param || {};
        param.type = param.type || "text";
        var html = [];
        switch (param.type) {
            case"text":
            default:
                var listHtml = "";
                var listId = "list_" + param.id;
                var attrList = "";
                html.push("<input ");
                if (param.datalist && param.datalist.length > 0) {
                    listHtml = (this.generateDatalist(param.datalist, listId));
                    html.push(" list='" + listId + "'")
                }
                html.push(param.attrHtml);
                html.push(" onchange='JsonFormHelper.onValueChane(this)' id='");
                html.push(param.id + "' type='" + param.type + "' ");
                html.push(param.attrName);
                html.push(" value='" + param.value + "' />");
                html.push(listHtml);
                break;
            case"textarea":
                html.push("<textarea ");
                html.push(param.attrHtml);
                html.push(" onchange='JsonFormHelper.addClass(this,\"json_form_dirty\")' id='");
                html.push(param.id + "'");
                html.push(param.attrName);
                html.push(">" + param.value + "</textarea>");
                break;
            case"hidden":
                html.push("<input type='hidden' ");
                html.push(param.attrHtml);
                html.push(" id='");
                html.push(param.id + "'");
                html.push(param.attrName);
                html.push(" value='" + param.value + "' />");
                break;
            case"select":
                html.push("<select ");
                html.push(param.attrHtml);
                html.push(" id='" + param.id + "' ");
                html.push(param.attrName);
                html.push(">");
                var list = param.datalist;
                var len = list.length;
                var isTextValue = len > 0 && typeof list[0] == "object";
                var valueArr = param.multiple ? param.value.toString().split(param.delimiter || this.defaultDelimiter) : [param.value];
                for (var i = 0; i < len; i++) {
                    var v = isTextValue ? list[i].value : list[i];
                    var t = isTextValue ? list[i].text : list[i];
                    html.push("<option " + (JsonFormHelper.isInArray(v.toString(), valueArr) ? "selected" : "") + " value='" + v + "'>" + t + "</option>")
                }
                html.push("</select>");
                break;
            case"radio":
                html.push("<span ");
                html.push(param.attrHtml);
                html.push(" id='" + param.id + "' ");
                html.push(">");
                var list = param.datalist;
                var len = list.length;
                var isTextValue = len > 0 && typeof list[0] == "object";
                for (var i = 0; i < len; i++) {
                    var v = isTextValue ? list[i].value : list[i];
                    var t = isTextValue ? list[i].text : list[i];
                    html.push("<label><input " + param.attrName + " type='radio' " + (v.toString() == param.value ? "checked" : "") + " value='" + v + "' />" + t + "</label>")
                }
                html.push("</span>");
                break;
            case"checkbox":
                html.push("<span ");
                html.push(param.attrHtml);
                html.push(" id='" + param.id + "' ");
                html.push(">");
                var list = param.datalist;
                var len = list.length;
                var isTextValue = len > 0 && typeof list[0] == "object";
                var valueArr = param.multiple ? param.value.toString().split(param.delimiter || this.defaultDelimiter) : [param.value];
                for (var i = 0; i < len; i++) {
                    var v = isTextValue ? list[i].value : list[i];
                    var t = isTextValue ? list[i].text : list[i];
                    html.push("<label><input " + param.attrName + " type='checkbox' " + (JsonFormHelper.isInArray(v.toString(), valueArr) ? "checked" : "") + " value='" + v + "' />" + t + "</label>")
                }
                html.push("</span>");
                break
        }
        return html.join("")
    }, createInputRow: function (param) {
        if (!param.fieldConfig) {
            param.fieldConfig = {}
        }
        var strAttrName = (typeof param.name_or_index == "string" ? ("name=" + param.name_or_index + "") : "");
        var elementId = param.fieldConfig.ctrlId || ("ele_json_" + JsonForm.renderCount);
        var labelHtml = "<label class='json_field_label label_" + param.name_or_index + "' style='" + (param.fieldConfig.labelCssText || "") + ";display:" + ((strAttrName == "" || param.hideLabel || param.fieldConfig.hideLabel) ? "none" : "") + "' for='" + elementId + "'>";
        labelHtml += this.getLabelText(param.fieldConfig, param.name_or_index) + ":";
        if (param.fieldConfig.required) {
            labelHtml += "<span class='json_form_required'>*</span>"
        }
        labelHtml += "</label>";
        var cssText = (param.fieldConfig.cssText || "");
        var attr = (param.fieldConfig.attr || "");
        if (param.fieldConfig.inline) {
            cssText += ";display:inline-block;*display:inline;*zoom:1"
        }
        if (param.fieldConfig.hidden) {
            attr += " hidden='hidden'";
            cssText += ";display:none"
        }
        if (param.fieldConfig.width) {
            cssText += ";width:" + param.fieldConfig.width
        }
        var html = ["<div " + attr + " style='" + cssText + "' class='json_form_element json_basic_element json_" + param.dataType + "'>"];
        html.push(labelHtml);
        var attrHtml = [];
        if (param.fieldConfig.ctrlAttr) {
            for (var p in param.fieldConfig.ctrlAttr) {
                attrHtml.push(p + "='" + param.fieldConfig.ctrlAttr[p] + "'")
            }
        }
        if (param.fieldConfig.required) {
            attrHtml.push("required")
        }
        if (param.fieldConfig.readonly) {
            attrHtml.push("readonly")
        }
        if (param.fieldConfig.disabled) {
            attrHtml.push("disabled")
        }
        if (param.fieldConfig.maxlength) {
            attrHtml.push("maxlength='" + param.fieldConfig.maxlength + "'")
        }
        if (param.fieldConfig.ctrlCssText) {
            attrHtml.push("style='" + param.fieldConfig.ctrlCssText + "'")
        }
        if (param.fieldConfig.pattern) {
            attrHtml.push("pattern='" + param.fieldConfig.pattern + "'")
        }
        if (param.fieldConfig.size) {
            attrHtml.push("size='" + param.fieldConfig.size + "'")
        }
        if (param.fieldConfig.type == "select" && param.fieldConfig.multiple) {
            attrHtml.push("multiple='multiple")
        }
        if (param.fieldConfig.title) {
            attrHtml.push("title='" + param.fieldConfig.title + "'")
        }
        if (param.fieldConfig.placeholder) {
            attrHtml.push("placeholder='" + param.fieldConfig.placeholder + "'")
        }
        attrHtml.push("class='json_field_input'");
        attrHtml = attrHtml.join(" ");
        if (param.dataType == "Boolean") {
            html.push("<input " + attrHtml + " id='" + elementId + "' type='checkbox' " + (param.inputData ? "checked" : "") + " " + strAttrName + " />")
        } else {
            html.push(FormElementFactory.generateInputHtml({type: param.fieldConfig.type, datalist: param.fieldConfig.datalist, attrHtml: attrHtml, multiple: param.fieldConfig.multiple, delimiter: param.fieldConfig.delimiter, id: elementId, attrName: strAttrName, value: param.inputData}))
        }
        if (param.fieldConfig.extHtml) {
            html.push(param.fieldConfig.extHtml)
        }
        if (!param.hiddenTips && param.fieldConfig.tips) {
            var tipsTpl = param.fieldConfig.tipsTpl || param.globalConfig.tipsTpl;
            html.push(tipsTpl.replace(/\{tips\}/g, param.fieldConfig.tips))
        }
        html.push("</div>");
        return html.join("")
    }, createString: function (inputStr, name_or_index, globalConfig, fieldConfig, hideLabel, hiddenTips) {
        return this.createInputRow({inputData: inputStr, dataType: "String", name_or_index: name_or_index, globalConfig: globalConfig, fieldConfig: fieldConfig, hideLabel: hideLabel, hiddenTips: hiddenTips})
    }, createNumber: function (inputNumber, name_or_index, globalConfig, fieldConfig, hideLabel, hiddenTips) {
        return this.createInputRow({inputData: inputNumber, dataType: "Number", name_or_index: name_or_index, globalConfig: globalConfig, fieldConfig: fieldConfig, hideLabel: hideLabel, hiddenTips: hiddenTips})
    }, createBoolean: function (inputBool, name_or_index, globalConfig, fieldConfig, hideLabel, hiddenTips) {
        return this.createInputRow({inputData: inputBool, dataType: "Boolean", name_or_index: name_or_index, globalConfig: globalConfig, fieldConfig: fieldConfig, hideLabel: hideLabel, hiddenTips: hiddenTips})
    }};
    JsonForm.renderCount = 0;
    function JsonForm(container, config) {
        this.container = typeof container == "string" ? document.getElementById(container) : container;
        this.config = {arrayIndex: {show: true, text: "序号"}, hideCollapser: false, addRowText: "增加", tipsTpl: '&nbsp;<a title="{tips}" href="#nolink">[?]</a>', thTipsTpl: "<sup title='{tips}'>[?]</sup>", fields: {}};
        if (typeof config == "object") {
            if (typeof config.arrayIndex == "object") {
                for (var p in config.arrayIndex) {
                    this.config.arrayIndex[p] = config.arrayIndex[p]
                }
            }
            for (var p in config) {
                if (p != "arrayIndex") {
                    this.config[p] = config[p]
                }
            }
        }
    }

    JsonForm.prototype.render = function (input) {
        this.container.innerHTML = this.renderData(input)
    };
    JsonForm.prototype.getJson = function (domEle) {
        var result = this.getJsonString();
        return eval("(" + result + ")")
    };
    JsonForm.prototype.getJsonString = function (domEle) {
        domEle = domEle || this.container.childNodes[0];
        var result = [];
        if (domEle.className.indexOf("json_Object") > -1) {
            domEleName = domEle.getAttribute("name");
            if (domEleName) {
                result.push('"' + domEleName + '":')
            }
            result.push("{");
            var childNodes = [];
            if (domEle.nodeName == "TR") {
                JsonFormHelper.each(domEle.cells, function (cell) {
                    if (cell.firstChild.nodeType == 1 && cell.firstChild.className.indexOf("json_form_element") > -1) {
                        childNodes.push(cell.firstChild)
                    }
                })
            } else {
                childNodes = domEle.getElementsByTagName("div")[0].childNodes
            }
            var len = childNodes.length;
            for (var i = 0; i < len; i++) {
                var node = childNodes[i];
                if (node.className.indexOf("json_form_element") > -1) {
                    result.push(this.getJsonString(node));
                    if (i < len - 1) {
                        result.push(",")
                    }
                }
            }
            result.push("}")
        } else {
            if (domEle.className.indexOf("json_Array") > -1) {
                domEleName = domEle.getAttribute("name");
                if (domEleName) {
                    result.push('"' + domEleName + '":')
                }
                result.push("[");
                var rows = domEle.tBodies.length > 0 ? domEle.tBodies[0].rows : [];
                var len = rows.length;
                for (var i = 0; i < len; i++) {
                    var row = rows[i];
                    result.push(this.getJsonString(row));
                    if (i < len - 1) {
                        result.push(",")
                    }
                }
                result.push("]")
            } else {
                if (domEle.className.indexOf("json_basic_element") > -1) {
                    var inputList = domEle.getElementsByTagName("input");
                    if (inputList.length == 0) {
                        inputList = domEle.getElementsByTagName("textarea")
                    }
                    if (inputList.length == 0) {
                        inputList = domEle.getElementsByTagName("select")
                    }
                    var l = inputList.length;
                    var fieldName = inputList[0].name;
                    var conf = this.config.fields[fieldName] || {};
                    if (fieldName) {
                        result.push('"' + fieldName + '":')
                    }
                    var values = [];
                    for (var i = 0; i < l; i++) {
                        var input = inputList[i];
                        var domType = input.type;
                        if (domType == "radio" || domType == "checkbox") {
                            if (!input.checked) {
                                continue
                            }
                        }
                        if (domType == "select-multiple") {
                            var select = input;
                            var selen = select.length;
                            for (j = 0; j < selen; j++) {
                                if (select.options[j].selected) {
                                    values.push(select.options[j].value)
                                }
                            }
                        } else {
                            values.push(input.value)
                        }
                    }
                    var tmpValue = values.join("");
                    if (conf.required && (tmpValue == "")) {
                        JsonFormHelper.handlerEmpty(inputList[0], conf);
                        throw new Error(fieldName + " cannot be empty")
                    }
                    if (tmpValue != "" && conf.pattern && !new RegExp("^" + conf.pattern + "$", "i").test(tmpValue)) {
                        JsonFormHelper.handlerInvalid(input, conf);
                        throw new Error("invalid value")
                    }
                    if (domEle.className.indexOf("json_Boolean") > -1) {
                        result.push(values.length > 0)
                    } else {
                        if (domEle.className.indexOf("json_Number") > -1) {
                            var len = values.length;
                            for (var i = 0; i < len; i++) {
                                values[i] -= 0
                            }
                            result.push(values.join(","))
                        } else {
                            if (domEle.className.indexOf("json_String") > -1) {
                                var len = values.length;
                                for (var i = 0; i < len; i++) {
                                    values[i] = values[i].replace(/\\/g, "\\\\");
                                    values[i] = values[i].replace(/"/g, '\\"');
                                    values[i] = values[i].replace(/\n/g, "\\n");
                                    values[i] = values[i].replace(/\t/g, "\\t")
                                }
                                result.push('"');
                                result.push(values.join(conf.delimiter || ","));
                                result.push('"')
                            }
                        }
                    }
                }
            }
        }
        result = result.join("");
        return result
    };
    JsonForm.prototype.renderData = function (input, name_or_index, hideLabel, hiddenTips) {
        if (input == null) {
            input = "null"
        }
        if (input == undefined) {
            input = "undefined"
        }
        JsonForm.renderCount++;
        var strAttrName = (typeof name_or_index == "string" ? ("name='" + name_or_index + "'") : "");
        var fieldConfig = this.config.fields[name_or_index] || {};
        fieldConfig.hideCollapser = "hideCollapser" in fieldConfig ? fieldConfig.hideCollapser : this.config.hideCollapser;
        fieldConfig.label = fieldConfig.label || this.config.label || name_or_index;
        switch (input.constructor) {
            case Number:
                return FormElementFactory.createNumber(input, name_or_index, this.config, fieldConfig, hideLabel, hiddenTips);
                break;
            case String:
                return FormElementFactory.createString(input, name_or_index, this.config, fieldConfig, hideLabel, hiddenTips);
                break;
            case Boolean:
                return FormElementFactory.createBoolean(input, name_or_index, this.config, fieldConfig, hideLabel, hiddenTips);
                break;
            case Object:
                var fdStyle = "";
                if (!name_or_index) {
                    fieldConfig.hideCollapser = true;
                    fdStyle = "border:none"
                }
                var fieldsetBegin = ("<fieldset " + strAttrName + " style='" + fdStyle + "' class='json_form_element json_Object'>");
                fieldsetBegin += "<legend " + (strAttrName == "" ? "style='display:'" : "") + ">";
                fieldsetBegin += "<label>";
                if (!fieldConfig.hideCollapser) {
                    fieldsetBegin += "<input checked onclick='JsonFormHelper.showNextSibling(this.parentNode.parentNode,this.checked)' type='checkbox' />"
                }
                fieldsetBegin += FormElementFactory.getLabelText(fieldConfig, name_or_index);
                fieldsetBegin += "</label></legend><div>";
                var fieldsetEnd = "</div></fieldset>";
                var temp = [fieldsetBegin];
                for (var key in input) {
                    temp.push(this.renderData(input[key], key))
                }
                temp.push(fieldsetEnd);
                return temp.join("");
                break;
            case Array:
                var temp = ["<table border='1' " + strAttrName + " class='json_form_element json_Array'>"];
                temp.push("<caption>");
                temp.push("<label>");
                if (!fieldConfig.hideCollapser) {
                    temp.push("<input checked onclick='JsonFormHelper.showNextSibling(this.parentNode.parentNode,this.checked);JsonFormHelper.showNextSibling(this.parentNode,this.checked)' type='checkbox' />")
                }
                temp.push(FormElementFactory.getLabelText(fieldConfig, name_or_index) + "</label>");
                if (!fieldConfig.noCreate) {
                    var addRowText = this.config.addRowText || "+";
                    temp.push(" <a class='json_form_action' href='javascript:void(null)' onclick='JsonFormHelper.addRow(this.parentNode.parentNode)' title='增加一行'>" + addRowText + "</a> ")
                }
                temp.push("</caption>");
                if(input!=null && input!==""){
                	var len = input.length;
                    var dd = input[0];
	                var isRegular = (len > 0 && input[0] && input[0].constructor == Object);
	                var attrIndexDisplay = this.config.arrayIndex.show ? "width:40px" : "display:none";
	                if (isRegular) {
	                    temp.push("<thead><tr>");
	                    temp.push("<th style='" + attrIndexDisplay + "'>" + this.config.arrayIndex.text + "</th>");
	                    var firstEle = input[0];
	                    for (var k in firstEle) {
	                        var fieldConf = this.config.fields[k];
	                        temp.push("<th>");
	                        temp.push(FormElementFactory.getLabelText(fieldConf, k));
	                        if (fieldConf && fieldConf.tips) {
	                            var tipsTpl = this.config.thTipsTpl;
	                            temp.push(tipsTpl.replace(/\{tips\}/g, fieldConf.tips))
	                        }
	                        temp.push("</th>")
	                    }
	                    if (!fieldConfig.noDelete) {
	                        temp.push("<th style='width:40px'>操作</th>")
	                    }
	                    temp.push("</tr></thead>")
	                }
	                for (var i = 0; i < len; i++) {
	                    var curEle = input[i];
                        if(!curEle){
                            break ;
                        }
	                    var eleType = JsonFormHelper.getObjType(curEle);
	                    var basicClass = JsonFormHelper.isInArray(eleType, ["String", "Number", "Boolean"]) ? " json_basic_element" : "";
	                    temp.push("<tr class='" + basicClass + "json_" + eleType + "'>");
	                    if (isRegular && curEle.constructor == Object) {
	                        temp.push("<td style='" + attrIndexDisplay + "' class='json_form_rowNumber'>" + (i + 1) + "</td>");
	                        for (var p in curEle) {
	                            temp.push("<td>");
	                            temp.push(this.renderData(curEle[p], p, true, true));
	                            temp.push("</td>")
	                        }
	                    } else {
	                        temp.push("<td class='json_form_rowNumber'>" + (i + 1) + "</td><td>");
	                        temp.push(this.renderData(curEle, i));
	                        temp.push("</td>")
	                    }
	                    if (!fieldConfig.noDelete) {
	                        temp.push("<td class='json_form_actionCell'><a class='json_form_action' href='javascript:void(null)' title='删除' onclick='if(!confirm(\"确定删除该行吗？\"))return false;var row = this.parentNode.parentNode;JsonFormHelper.removeRow(row);'>×<a></td>")
	                    }
	                    temp.push("</tr>")
	                }
	                temp.push("</table>");
	                return temp.join("");
	                break
                }
        }
    };
    window.JsonForm = JsonForm;
    window.JsonFormHelper = JsonFormHelper
})();
