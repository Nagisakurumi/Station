package com.station.martian.mapper;

import com.station.martian.model.User;
import java.util.List;
import org.apache.ibatis.annotations.Delete;
import org.apache.ibatis.annotations.Insert;
import org.apache.ibatis.annotations.Result;
import org.apache.ibatis.annotations.Results;
import org.apache.ibatis.annotations.Select;
import org.apache.ibatis.annotations.SelectKey;
import org.apache.ibatis.annotations.Update;
import org.apache.ibatis.type.JdbcType;

public interface UserMapper {
    @Delete({
        "delete from user",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    int deleteByPrimaryKey(Integer id);

    @Insert({
        "insert into user (Account, Password, ",
        "NickName, CreateTime, ",
        "Email, Phone, LastLoginDate, ",
        "LastBuyDate)",
        "values (#{account,jdbcType=VARCHAR}, #{password,jdbcType=VARCHAR}, ",
        "#{nickname,jdbcType=VARCHAR}, #{createtime,jdbcType=TIMESTAMP}, ",
        "#{email,jdbcType=VARCHAR}, #{phone,jdbcType=VARCHAR}, #{lastlogindate,jdbcType=TIMESTAMP}, ",
        "#{lastbuydate,jdbcType=TIMESTAMP})"
    })
    @SelectKey(statement="SELECT LAST_INSERT_ID()", keyProperty="id", before=false, resultType=Integer.class)
    int insert(User record);

    @Select({
        "select",
        "Id, Account, Password, NickName, CreateTime, Email, Phone, LastLoginDate, LastBuyDate",
        "from user",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    @Results({
        @Result(column="Id", property="id", jdbcType=JdbcType.INTEGER, id=true),
        @Result(column="Account", property="account", jdbcType=JdbcType.VARCHAR),
        @Result(column="Password", property="password", jdbcType=JdbcType.VARCHAR),
        @Result(column="NickName", property="nickname", jdbcType=JdbcType.VARCHAR),
        @Result(column="CreateTime", property="createtime", jdbcType=JdbcType.TIMESTAMP),
        @Result(column="Email", property="email", jdbcType=JdbcType.VARCHAR),
        @Result(column="Phone", property="phone", jdbcType=JdbcType.VARCHAR),
        @Result(column="LastLoginDate", property="lastlogindate", jdbcType=JdbcType.TIMESTAMP),
        @Result(column="LastBuyDate", property="lastbuydate", jdbcType=JdbcType.TIMESTAMP)
    })
    User selectByPrimaryKey(Integer id);

    @Select({
        "select",
        "Id, Account, Password, NickName, CreateTime, Email, Phone, LastLoginDate, LastBuyDate",
        "from user"
    })
    @Results({
        @Result(column="Id", property="id", jdbcType=JdbcType.INTEGER, id=true),
        @Result(column="Account", property="account", jdbcType=JdbcType.VARCHAR),
        @Result(column="Password", property="password", jdbcType=JdbcType.VARCHAR),
        @Result(column="NickName", property="nickname", jdbcType=JdbcType.VARCHAR),
        @Result(column="CreateTime", property="createtime", jdbcType=JdbcType.TIMESTAMP),
        @Result(column="Email", property="email", jdbcType=JdbcType.VARCHAR),
        @Result(column="Phone", property="phone", jdbcType=JdbcType.VARCHAR),
        @Result(column="LastLoginDate", property="lastlogindate", jdbcType=JdbcType.TIMESTAMP),
        @Result(column="LastBuyDate", property="lastbuydate", jdbcType=JdbcType.TIMESTAMP)
    })
    List<User> selectAll();

    @Update({
        "update user",
        "set Account = #{account,jdbcType=VARCHAR},",
          "Password = #{password,jdbcType=VARCHAR},",
          "NickName = #{nickname,jdbcType=VARCHAR},",
          "CreateTime = #{createtime,jdbcType=TIMESTAMP},",
          "Email = #{email,jdbcType=VARCHAR},",
          "Phone = #{phone,jdbcType=VARCHAR},",
          "LastLoginDate = #{lastlogindate,jdbcType=TIMESTAMP},",
          "LastBuyDate = #{lastbuydate,jdbcType=TIMESTAMP}",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    int updateByPrimaryKey(User record);
}