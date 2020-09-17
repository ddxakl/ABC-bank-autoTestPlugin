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
    if (window.flag) {
      let _this = this;
      this.$children && getChildrens(this.$children, _this);
    }
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
    if (window.flag) {
      var tag = vm.$options._componentTag;
      if (eventName === 'click' && vm._events.click != undefined) {
        console.log("click事件", vm);
        getControlInfo(vm, eventName, tag, payload, window.recContext);
      } else if (eventName === 'blur') {
        if (tag === "aui-text-input" || tag === 'aui-input' || tag === 'aui-textarea' || tag === 'aui-search'|| tag === 'aui-currency-input') {
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
      else if(eventName==="tab-changed"){
        if(tag === 'aui-tab'){
          setTimeout(() => {
            getControlInfo(vm, eventName, tag, payload, window.recContext);
          }, 100);
        }
      }
    }
  }
}
function getControlInfo(vm, eventName, tag, payload, recContext) {
  let controlInfo= {};
  cordova.exec(result => window.tradeCode=result, error => console.error(error), 'TradeContextProvider', 'GetTradeCode', []);
  console.log("获取到的交易码是"+window.tradeCode);
  controlInfo['tradeCode']=window.tradeCode;
  controlInfo['pageName'] = vm.$vnode.context.constructor.extendOptions.name;
  controlInfo['controlRef'] = vm.$vnode.data.ref;
  controlInfo['controlModel'] = vm.$vnode.data.model === undefined ? "" : vm.$vnode.data.model.expression;
  controlInfo['controlType'] = "";
  controlInfo['controlvalue'] = "";
  controlInfo['addiValue'] = "";
  controlInfo['eventName'] = eventName;

  if (tag === "aui-button" || tag === "submit-button") {
    console.log("组件的类型为button");
    controlInfo['controlType'] = "AButton";
  } else if (tag === "aui-input" || tag === "aui-text-input") {
    controlInfo['controlType'] = "AText";
    controlInfo['controlvalue'] = vm.$vnode.data.model === undefined ? vm.value : vm.$vnode.data.model.value;
  } else if (tag === 'aui-textarea') {
    controlInfo['controlType'] = "MultiLineText";
    controlInfo['controlvalue'] = vm.$vnode.data.model === undefined ? vm.value : vm.$vnode.data.model.value;
  } else if (tag === 'aui-selector' || tag === 'aui-select') {
    controlInfo['controlType'] = "ASelect";
    controlInfo['controlvalue'] = vm.$vnode.data.model === undefined ? vm.value : vm.$vnode.data.model.value;
  } else if (tag === 'aui-img') {
    controlInfo['controlType'] = "AImage";
  } else if (tag === 'aui-datetime' || tag === 'aui-date-key-picker') {
    controlInfo['controlType'] = "ADateText";
    controlInfo['controlvalue'] = vm.$vnode.data.model === undefined ? vm.value : vm.$vnode.data.model.value;
  } else if (tag === 'aui-actionsheet') {
    controlInfo['controlType'] = 'AActionSheet';
    controlInfo['controlvalue'] = "";
  } else if (tag === 'aui-datetime-range') {
    controlInfo['controlType'] = "ADateRange";
    controlInfo['controlvalue'] = "";
  } else if (tag === 'aui-datetime-view') {
    controlInfo['controlType'] = "ADateView";
    controlInfo['controlvalue'] = "";
  } else if (tag === 'aui-calendar') {
    controlInfo['controlType'] = "Calendar";
  } else if (tag === 'aui-inline-calendar') {
    controlInfo['controlType'] = "InlineCalendar";
  } else if (tag === 'aui-checker') {
    let type = vm.type;
    if (type === "radio") {
      controlInfo['controlType'] = "ARadioButton";
    } else if (type === "checkbox") {
      controlInfo['controlType'] = "ACheckbox";
    }
    controlInfo['controlvalue'] = vm.$vnode.data.model === undefined ? vm.value : vm.$vnode.data.model.value;
  } else if (tag === 'aui-radio' || tag === 'aui-radio-group') {
    controlInfo['controlType'] = "ARadioButton";
    controlInfo['controlvalue'] = vm.$vnode.data.model === undefined ? vm.value : vm.$vnode.data.model.value;
  } else if (tag === 'aui-checklist') {
    controlInfo['controlType'] = "ACheckList";
    controlInfo['controlvalue'] = vm.$vnode.data.model === undefined ? vm.value : vm.$vnode.data.model.value;
  } else if (tag === 'aui-check-icon') {
    controlInfo['controlType'] = "ACheckIcon";
    controlInfo['controlvalue'] = "";
  } else if (tag === 'aui-popup-radio') {
    controlInfo['controlType'] = "APopupRadio";
    controlInfo['controlvalue'] = "";
  } else if (tag === 'aui-search') {
    controlInfo['controlType'] = 'ASearch';
    controlInfo['controlvalue'] = "";
  } else if (tag === 'aui-tab') {
    controlInfo['controlType'] = 'ATab';
    controlInfo['controlvalue']=vm.activeIndex;
  } else if (tag === 'aui-button-tab') {
    controlInfo['controlType'] = 'ButtonTab';
  } else if (tag === 'aui-tabbar') {
    controlInfo['controlType'] = 'ATabbar';
  } else if (tag === 'aui-address') {
    controlInfo['controlType'] = 'AAddress';
    controlInfo['controlvalue'] = "";
  } else if (tag === 'aui-picker') {
    controlInfo['controlType'] = "APicker";
  } else if (tag === 'aui-color-picker') {
    controlInfo['controlType'] = "AColorPicker";
  } else if (tag === 'aui-popup-picker') {
    controlInfo['controlType'] = "APopupPicker";
  } else if (tag === 'aui-range') {
    controlInfo['controlType'] = "Range";
  } else if (tag === 'aui-rate') {
    controlInfo['controlType'] = "ARate";
  } else if (tag === 'aui-flexbox') {
    controlInfo['controlType'] = 'AFlexBox';
    controlInfo['controlvalue'] = vm.$vnode.data.key;
    controlInfo['addiValue'] = 'AFB';
  } else if (tag === 'aui-cell') {
    controlInfo['controlType'] = 'ACell';
  } else if (tag === 'aui-alert') {
    controlInfo['controlType'] = 'AAlert';
    controlInfo['controlvalue'] = vm.showValue;
  } else if (tag === 'aui-confirm') {
    controlInfo['controlType'] = 'AConfirm';
    controlInfo['controlvalue'] = vm.msg;
    if (eventName === 'confirm') {
      controlInfo['addiValue'] = 'Atrue';
    } else if (eventName === 'cancel') {
      controlInfo['addiValue'] = 'Afalse';
    }
  }
  let controlMap = {};
  controlMap['tradeCode'] = controlInfo['tradeCode'];
  controlMap['pageCode'] = controlInfo['pageName'];
  controlMap['controlType'] = controlInfo['controlType'];
  controlMap['id'] = controlInfo['controlRef'];
  controlMap['value'] = controlInfo['controlvalue'];
  controlMap['valueAdd'] = controlInfo['addiValue'];
  controlMap['itemName'] = controlInfo['controlRef'];
  controlMap["eventName"]=controlInfo['eventName'];
  console.log("vue组件");
  console.log(JSON.stringify(controlMap));
  //点击一次录制一次记录一次
  window.controlMap = controlMap;
  ABCAutoTest.getRecContext(JSON.stringify(window.controlMap));
  //点击录制完成之后再上传 
  //recContext.push(controlMap);
}
function addvalue(result){
  console.log(result);
  window.tradeCode=result;
}