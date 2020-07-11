package com.station.martian.mapper;

import com.station.martian.model.Userinfo;
import java.util.List;
import org.apache.ibatis.annotations.Delete;
import org.apache.ibatis.annotations.Insert;
import org.apache.ibatis.annotations.Result;
import org.apache.ibatis.annotations.Results;
import org.apache.ibatis.annotations.Select;
import org.apache.ibatis.annotations.SelectKey;
import org.apache.ibatis.annotations.Update;
import org.apache.ibatis.type.JdbcType;

public interface UserinfoMapper {
    @Delete({
        "delete from userinfo",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    int deleteByPrimaryKey(Integer id);

    @Insert({
        "insert into userinfo (UserId, UserGroupId)",
        "values (#{userid,jdbcType=INTEGER}, #{usergroupid,jdbcType=INTEGER})"
    })
    @SelectKey(statement="SELECT LAST_INSERT_ID()", keyProperty="id", before=false, resultType=Integer.class)
    int insert(Userinfo record);

    @Select({
        "select",
        "Id, UserId, UserGroupId",
        "from userinfo",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    @Results({
        @Result(column="Id", property="id", jdbcType=JdbcType.INTEGER, id=true),
        @Result(column="UserId", property="userid", jdbcType=JdbcType.INTEGER),
        @Result(column="UserGroupId", property="usergroupid", jdbcType=JdbcType.INTEGER)
    })
    Userinfo selectByPrimaryKey(Integer id);

    @Select({
        "select",
        "Id, UserId, UserGroupId",
        "from userinfo"
    })
    @Results({
        @Result(column="Id", property="id", jdbcType=JdbcType.INTEGER, id=true),
        @Result(column="UserId", property="userid", jdbcType=JdbcType.INTEGER),
        @Result(column="UserGroupId", property="usergroupid", jdbcType=JdbcType.INTEGER)
    })
    List<Userinfo> selectAll();

    @Update({
        "update userinfo",
        "set UserId = #{userid,jdbcType=INTEGER},",
          "UserGroupId = #{usergroupid,jdbcType=INTEGER}",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    int updateByPrimaryKey(Userinfo record);
}