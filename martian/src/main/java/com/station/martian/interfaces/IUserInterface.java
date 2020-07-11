package com.station.martian.interfaces;

import com.github.pagehelper.PageInfo;
import com.station.martian.model.User;

public interface IUserInterface {
    /**
     * 获取用户根据id
     * @param id  用户id
     * @return
     */
    public User getUserById(int id);

    /**
     * 注册一个用户
     * @param user 用户信息
     * @return
     */
    public boolean registerUser(User user);

    /**
     * 修改用户信息
     * @param user
     * @return
     */
    public boolean modifyUser(User user);

    /**
     * 查找用户根据用户名称
     * @param userName 用户账号
     * @return
     */
    public User getUserByUserAccount(String userName);

    /**
     * 分页查询用户列表
     * @param page
     * @param size
     * @return
     */
    public PageInfo<User> getUserList(int page, int size);
}
