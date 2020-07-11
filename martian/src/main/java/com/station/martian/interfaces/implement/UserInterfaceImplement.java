package com.station.martian.interfaces.implement;

import com.github.pagehelper.PageHelper;
import com.github.pagehelper.PageInfo;
import com.station.martian.interfaces.IUserInterface;
import com.station.martian.mapper.UserMapper;
import com.station.martian.model.User;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.PageRequest;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class UserInterfaceImplement implements IUserInterface {

    @Autowired
    private UserMapper userMapper;

    @Override
    public User getUserById(int id) {
        return userMapper.selectByPrimaryKey(id);
    }

    @Override
    public boolean registerUser(User user) {
        int res = userMapper.insert(user);
        return res > 0;
    }

    @Override
    public boolean modifyUser(User user) {
        int res = userMapper.updateByPrimaryKey(user);
        return res > 0;
    }

    /**
     * 获取用户 根据账号
     * @param userName 用户账号
     * @return
     */
    @Override
    public User getUserByUserAccount(String userName) {
        return null;
    }

    @Override
    public PageInfo<User> getUserList(int page, int size) {
        PageHelper.startPage(page, size);
        List<User> users = userMapper.selectAll();
        PageInfo<User> pageInfo = new PageInfo<>(users);
        return pageInfo;
    }

}
