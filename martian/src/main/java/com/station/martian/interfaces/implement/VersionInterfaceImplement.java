package com.station.martian.interfaces.implement;

import com.station.martian.interfaces.IVersionInterface;
import com.station.martian.mapper.VersionMapper;
import com.station.martian.model.Version;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class VersionInterfaceImplement implements IVersionInterface {

    /**
     * 版本操作mapper
     */
    @Autowired
    private VersionMapper versionMapper;

    @Override
    public Version getVersionById(int id) {
        return versionMapper.selectByPrimaryKey(id);
    }

    @Override
    public Version getVersionByVersionNumber(String version) {
        return null;
    }

    @Override
    public boolean addVersion(Version version) {
        int res = versionMapper.insert(version);
        return res > 0;
    }

    @Override
    public boolean modifyVersion(Version version) {
        int res = versionMapper.updateByPrimaryKey(version);
        return res > 0;
    }
}
