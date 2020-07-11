package com.station.martian.mapper;

import com.station.martian.model.Roleinfo;
import java.util.List;
import org.apache.ibatis.annotations.Delete;
import org.apache.ibatis.annotations.Insert;
import org.apache.ibatis.annotations.Result;
import org.apache.ibatis.annotations.Results;
import org.apache.ibatis.annotations.Select;
import org.apache.ibatis.annotations.SelectKey;
import org.apache.ibatis.annotations.Update;
import org.apache.ibatis.type.JdbcType;

public interface RoleinfoMapper {
    @Delete({
        "delete from roleinfo",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    int deleteByPrimaryKey(Integer id);

    @Insert({
        "insert into roleinfo (RoleId, UserGroupId)",
        "values (#{roleid,jdbcType=INTEGER}, #{usergroupid,jdbcType=INTEGER})"
    })
    @SelectKey(statement="SELECT LAST_INSERT_ID()", keyProperty="id", before=false, resultType=Integer.class)
    int insert(Roleinfo record);

    @Select({
        "select",
        "Id, RoleId, UserGroupId",
        "from roleinfo",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    @Results({
        @Result(column="Id", property="id", jdbcType=JdbcType.INTEGER, id=true),
        @Result(column="RoleId", property="roleid", jdbcType=JdbcType.INTEGER),
        @Result(column="UserGroupId", property="usergroupid", jdbcType=JdbcType.INTEGER)
    })
    Roleinfo selectByPrimaryKey(Integer id);

    @Select({
        "select",
        "Id, RoleId, UserGroupId",
        "from roleinfo"
    })
    @Results({
        @Result(column="Id", property="id", jdbcType=JdbcType.INTEGER, id=true),
        @Result(column="RoleId", property="roleid", jdbcType=JdbcType.INTEGER),
        @Result(column="UserGroupId", property="usergroupid", jdbcType=JdbcType.INTEGER)
    })
    List<Roleinfo> selectAll();

    @Update({
        "update roleinfo",
        "set RoleId = #{roleid,jdbcType=INTEGER},",
          "UserGroupId = #{usergroupid,jdbcType=INTEGER}",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    int updateByPrimaryKey(Roleinfo record);
}