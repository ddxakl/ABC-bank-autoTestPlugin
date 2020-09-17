/**
 * Created by zhaoyuchen on 2019/01/07.
 * 交易录制时,aui组件事件监听
 */
import {ABCAutoTest} from "./ABCTest";
const internalRE = /^(?:pre-)?hook:/;
export default {
  install(Vue, pluginOption = {}) {
    var method = pluginOption.type;
    const original = Vue.prototype[method];
    if (original) {
      Vue.prototype[method] = function (...args) {
        const res = original.apply(this, args);
        auiEventListeners(this, args[0], args.slice(1));
        return res;
      };
    }
  },
  mounted() {
    let _this = this;
    this.$children && getChildrens(this.$children, _this);
  }
};
function getChildrens(childrens, _this) {
  for (let i = 0; i < childrens.length; i++) {
    getNativeEvent(childrens[i], _this);
    childrens[i].$children && getChildrens(childrens[i].$children, _this);
  }
}

function getNativeEvent(vm, _this) {
  if (vm._vnode.data && vm._vnode.data.on && vm._vnode.data.on.click) {
    vm._vnode.elm.addEventListener("click", function() {
      getControlInfo(vm, "", vm.$options._componentTag, "", window.recContext);
    });
  }
}

function auiEventListeners(vm, eventName, payload) {
  if (typeof eventName === 'string' && !internalRE.test(eventName)) {
    console.log("eventName:", eventName);
    //console.log("当前点击的组件是："+vm);
    var tag = vm.$options._componentTag;
    if(eventName === 'focus'){
        window.StartTime = new Date();//组件聚焦获取组件开始操作的时间
    }else if (eventName === 'click' && vm._events.click != undefined) {
      console.log("click事件", vm);
      getControlInfo(vm, eventName, tag, payload, window.recContext);
    } else if (eventName === 'blur') {
      if (tag === "aui-text-input" || tag === 'aui-input' || tag === 'aui-textarea' || tag === 'aui-search') {
        console.log("blur事件", vm);
        getControlInfo(vm, eventName, tag, payload, window.recContext);
      }
    } else if (eventName === 'change') {
      if (tag === 'aui-select' || tag === 'aui-selector' || tag === 'aui-datetime' || tag === 'aui-radio-group') {
        console.log("change事件", vm);
        getControlInfo(vm, eventName, tag, payload, window.recContext);
      }
    } else if (eventName === 'update:value') {
      if (tag === 'aui-check-icon') {
        //getControlInfo(vm, eventName, tag, payload, window.recContext);
      }
    } else if (eventName === 'click-menu') {
      if (tag === 'aui-actionsheet') {
        //getControlInfo(vm, eventName, tag, payload, window.recContext);
      }
    }
  }
}

function getControlInfo(vm, eventName, tag, payload, recContext) {
  let controlInfo = {};
  controlInfo['tradeCode'] = vm.$vnode.context.constructor.extendOptions.name;
  controlInfo['pageName'] = vm.$vnode.context.constructor.extendOptions.name;
  controlInfo['controlRef'] = vm.$vnode.data.ref;
  controlInfo['controlModel'] = vm.$vnode.data.model === undefined ? "" : vm.$vnode.data.model.expression;
  controlInfo['controlType'] = "";
  controlInfo['controlvalue'] = "";
  controlInfo['addiValue'] = "";
  controlInfo['handleStartTime']=window.StartTime;
  controlInfo['handleStopTime']=new Date();//组件失焦获取停止操作组件的时间

  if (tag === "aui-button" || tag === "submit-button") {
    console.log("组件的类型为button");
    controlInfo['controlType'] = "Button";
  } else if (tag === "aui-input" || tag === "aui-text-input") {
    controlInfo['controlType'] = "Text";
    controlInfo['controlvalue'] = vm.$vnode.data.model === undefined ? vm.value : vm.$vnode.data.model.value;
  } else if (tag === 'aui-textarea') {
    controlInfo['controlType'] = "MultiLineText";
    controlInfo['controlvalue'] = vm.$vnode.data.model === undefined ? vm.value : vm.$vnode.data.model.value;
  } else if (tag === 'aui-selector' || tag === 'aui-select') {
    controlInfo['controlType'] = "Select";
    controlInfo['controlvalue'] = vm.$vnode.data.model === undefined ? vm.value : vm.$vnode.data.model.value;
  } else if (tag === 'aui-img') {
    controlInfo['controlType'] = "Image";
  } else if (tag === 'aui-datetime' || tag === 'aui-date-key-picker') {
    controlInfo['controlType'] = "DateText";
    controlInfo['controlvalue'] = vm.$vnode.data.model === undefined ? vm.value : vm.$vnode.data.model.value;
  } else if (tag === 'aui-actionsheet') {
    controlInfo['controlType'] = 'ActionSheet';
    controlInfo['controlvalue'] = "";
  } else if (tag === 'aui-datetime-range') {
    controlInfo['controlType'] = "DateRange";
    controlInfo['controlvalue'] = "";
  } else if (tag === 'aui-datetime-view') {
    controlInfo['controlType'] = "DateView";
    controlInfo['controlvalue'] = "";
  } else if (tag === 'aui-calendar') {
    controlInfo['controlType'] = "Calendar";
  } else if (tag === 'aui-inline-calendar') {
    controlInfo['controlType'] = "InlineCalendar";
  } else if (tag === 'aui-checker') {
    let type = vm.type;
    if (type === "radio") {
      controlInfo['controlType'] = "RadioButton";
    } else if (type === "checkbox") {
      controlInfo['controlType'] = "Checkbox";
    }
    controlInfo['controlvalue'] = vm.$vnode.data.model === undefined ? vm.value : vm.$vnode.data.model.value;
  } else if (tag === 'aui-radio' || tag === 'aui-radio-group') {
    controlInfo['controlType'] = "RadioButton";
    controlInfo['controlvalue'] = vm.$vnode.data.model === undefined ? vm.value : vm.$vnode.data.model.value;
  } else if (tag === 'aui-checklist') {
    controlInfo['controlType'] = "CheckList";
    controlInfo['controlvalue'] = vm.$vnode.data.model === undefined ? vm.value : vm.$vnode.data.model.value;
  } else if (tag === 'aui-check-icon') {
    controlInfo['controlType'] = "CheckIcon";
    controlInfo['controlvalue'] = "";
  } else if (tag === 'aui-popup-radio') {
    controlInfo['controlType'] = "PopupRadio";
    controlInfo['controlvalue'] = "";
  } else if (tag === 'aui-search') {
    controlInfo['controlType'] = 'Search';
    controlInfo['controlvalue'] = "";
  } else if (tag === 'aui-tab') {
    controlInfo['controlType'] = 'Tab';
  } else if (tag === 'aui-button-tab') {
    controlInfo['controlType'] = 'ButtonTab';
  } else if (tag === 'aui-tabbar') {
    controlInfo['controlType'] = 'Tabbar';
  } else if (tag === 'aui-address') {
    controlInfo['controlType'] = 'Address';
    controlInfo['controlvalue'] = "";
  } else if (tag === 'aui-picker') {
    controlInfo['controlType'] = "Picker";
  } else if (tag === 'aui-color-picker') {
    controlInfo['controlType'] = "ColorPicker";
  } else if (tag === 'aui-popup-picker') {
    controlInfo['controlType'] = "PopupPicker";
  } else if (tag === 'aui-range') {
    controlInfo['controlType'] = "Range";
  } else if (tag === 'aui-rate') {
    controlInfo['controlType'] = "Rate";
  } else if (tag === 'aui-flexbox') {
    controlInfo['controlType'] = 'FlexBox';
    controlInfo['controlvalue'] = vm.$vnode.data.key;
    controlInfo['addiValue'] = 'FB';
  } else if (tag === 'aui-cell') {
    controlInfo['controlType'] = 'Cell';
  } else if (tag === 'aui-alert') {
    controlInfo['controlType'] = 'Alert';
    controlInfo['controlvalue'] = vm.showValue;
  } else if (tag === 'aui-confirm') {
    controlInfo['controlType'] = 'Confirm';
    controlInfo['controlvalue'] = vm.msg;
    if (eventName === 'confirm') {
      controlInfo['addiValue'] = 'true';
    } else if (eventName === 'cancel') {
      controlInfo['addiValue'] = 'false';
    }
  }
  let controlMap = {};
  controlMap['tradeCode'] = controlInfo['tradeCode'];
  controlMap['pageCode'] = controlInfo['pageName'];
  controlMap['controlType'] = controlInfo['controlType'];
  controlMap['id'] = controlInfo['controlRef'] + "#" + controlInfo['controlModel'];
  console.log("组件标签："+controlMap['id'] );
  controlMap['value'] = controlInfo['controlvalue'];
  controlMap['valueAdd'] = controlInfo['addiValue'];
  console.log("vue组件");
  console.log(JSON.stringify(controlMap));
  //点击一次录制一次记录一次
  window.controlMap = controlMap;

  var name = controlInfo['pageName'];
  for (var i = window.tradeInfo['tradePageList'].length - 1; i >= 0; i--) {
    console.log(name+'------------------------'+window.tradeInfo['tradePageList'][i]['pageName']);
    if (name === window.tradeInfo['tradePageList'][i]['pageName']) {
     // pageControlInfo['controlNum'] = window.tradeInfo['tradePageList'][i]['pageControlList'].length + 1;
      window.tradeInfo['tradePageList'][i]['pageControlList'].push(controlInfo);
      console.log("pageControlList的长度----"+ window.tradeInfo['tradePageList'][i]['pageControlList'].length);
      break;
    }
  }

  //点击录制完成之后再上传 
  //recContext.push(controlMap);
}
