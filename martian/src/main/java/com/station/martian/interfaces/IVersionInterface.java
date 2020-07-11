package com.station.martian.interfaces;

import com.station.martian.model.Version;

/**
 * 版本信息操作接口
 */
public interface IVersionInterface {
    /**
     * 根据id获取版本信息
     * @param id
     * @return
     */
    public Version getVersionById(int id);

    /**
     * 根据版本号获取版本
     * @param version
     * @return
     */
    public Version getVersionByVersionNumber(String version);

    /**
     * 添加一个版本号
     * @param version
     * @return
     */
    public boolean addVersion(Version version);

    /**
     * 修改版本信息
     * @param version
     * @return
     */
    public boolean modifyVersion(Version version);
}
