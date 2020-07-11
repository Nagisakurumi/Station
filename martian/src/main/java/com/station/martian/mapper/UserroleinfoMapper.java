package com.station.martian.mapper;

import com.station.martian.model.Userroleinfo;
import java.util.List;
import org.apache.ibatis.annotations.Delete;
import org.apache.ibatis.annotations.Insert;
import org.apache.ibatis.annotations.Result;
import org.apache.ibatis.annotations.Results;
import org.apache.ibatis.annotations.Select;
import org.apache.ibatis.annotations.SelectKey;
import org.apache.ibatis.annotations.Update;
import org.apache.ibatis.type.JdbcType;

public interface UserroleinfoMapper {
    @Delete({
        "delete from userroleinfo",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    int deleteByPrimaryKey(Integer id);

    @Insert({
        "insert into userroleinfo (UserId, RoleId)",
        "values (#{userid,jdbcType=INTEGER}, #{roleid,jdbcType=INTEGER})"
    })
    @SelectKey(statement="SELECT LAST_INSERT_ID()", keyProperty="id", before=false, resultType=Integer.class)
    int insert(Userroleinfo record);

    @Select({
        "select",
        "Id, UserId, RoleId",
        "from userroleinfo",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    @Results({
        @Result(column="Id", property="id", jdbcType=JdbcType.INTEGER, id=true),
        @Result(column="UserId", property="userid", jdbcType=JdbcType.INTEGER),
        @Result(column="RoleId", property="roleid", jdbcType=JdbcType.INTEGER)
    })
    Userroleinfo selectByPrimaryKey(Integer id);

    @Select({
        "select",
        "Id, UserId, RoleId",
        "from userroleinfo"
    })
    @Results({
        @Result(column="Id", property="id", jdbcType=JdbcType.INTEGER, id=true),
        @Result(column="UserId", property="userid", jdbcType=JdbcType.INTEGER),
        @Result(column="RoleId", property="roleid", jdbcType=JdbcType.INTEGER)
    })
    List<Userroleinfo> selectAll();

    @Update({
        "update userroleinfo",
        "set UserId = #{userid,jdbcType=INTEGER},",
          "RoleId = #{roleid,jdbcType=INTEGER}",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    int updateByPrimaryKey(Userroleinfo record);
}