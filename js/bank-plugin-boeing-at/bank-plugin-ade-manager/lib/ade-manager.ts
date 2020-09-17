const ADE_MANAGER_PLUGIN_NAME = 'AdeManager';
const ADE_MANAGER_PLUGIN_GETADE_ACTION = "GetAde";

/**
 * 数据字典条目管理器
 */
export default class AdeManager {

    private static adeRegistry = new Map<string, object>();

    /**
    * 获取数据字典条目信息
    * @param adeKey 数据字典条目名称
    */
    static getAdeAsync(adeKey: string): Promise<any> {
        const adeName = AdeManager.resolveAdeName(adeKey);
        // 如果本地缓存有，则从本地缓存拿
        if (AdeManager.adeRegistry[adeName]) {
            return Promise.resolve(AdeManager.adeRegistry[adeName]);
        } else {
            // 否则从native层再获取
            return new Promise((resolve, reject) => {
                cordova.exec(
                    result => {
                        const ade = JSON.parse(result.Content);
                        AdeManager.adeRegistry.set(ade.Name, ade);
                        resolve(ade);
                    },
                    error => {
                        reject(error);
                    },
                    ADE_MANAGER_PLUGIN_NAME,
                    ADE_MANAGER_PLUGIN_GETADE_ACTION,
                    [adeKey]);
            });
        }
    }

    /**
     * 解析数据字典项路径
     * @param adePath 数据字典项路径
     * @returns 数据字典名称
     */
    static resolveAdeName(adePath: string) {
        if (adePath.indexOf('/') !== -1) {
            const slashIndex = adePath.lastIndexOf('/');
            const adeName = adePath.substring(slashIndex + 1);
            // 去掉.ade
            return adeName.substring(0, adeName.length - 4);
        }
        return adePath;
    }
}

