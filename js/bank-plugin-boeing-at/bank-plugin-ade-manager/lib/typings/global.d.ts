declare namespace cordova {
    function exec(
        resultCallback: (result) => void,
        errorCallback: (error) => void,
        pluginName: string,
        actionName: string,
        ...parameters: any[]);
} 