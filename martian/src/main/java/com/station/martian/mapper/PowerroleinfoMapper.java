package com.station.martian.mapper;

import com.station.martian.model.Powerroleinfo;
import java.util.List;
import org.apache.ibatis.annotations.Delete;
import org.apache.ibatis.annotations.Insert;
import org.apache.ibatis.annotations.Result;
import org.apache.ibatis.annotations.Results;
import org.apache.ibatis.annotations.Select;
import org.apache.ibatis.annotations.SelectKey;
import org.apache.ibatis.annotations.Update;
import org.apache.ibatis.type.JdbcType;

public interface PowerroleinfoMapper {
    @Delete({
        "delete from powerroleinfo",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    int deleteByPrimaryKey(Integer id);

    @Insert({
        "insert into powerroleinfo (PowerId, RoleId)",
        "values (#{powerid,jdbcType=INTEGER}, #{roleid,jdbcType=INTEGER})"
    })
    @SelectKey(statement="SELECT LAST_INSERT_ID()", keyProperty="id", before=false, resultType=Integer.class)
    int insert(Powerroleinfo record);

    @Select({
        "select",
        "Id, PowerId, RoleId",
        "from powerroleinfo",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    @Results({
        @Result(column="Id", property="id", jdbcType=JdbcType.INTEGER, id=true),
        @Result(column="PowerId", property="powerid", jdbcType=JdbcType.INTEGER),
        @Result(column="RoleId", property="roleid", jdbcType=JdbcType.INTEGER)
    })
    Powerroleinfo selectByPrimaryKey(Integer id);

    @Select({
        "select",
        "Id, PowerId, RoleId",
        "from powerroleinfo"
    })
    @Results({
        @Result(column="Id", property="id", jdbcType=JdbcType.INTEGER, id=true),
        @Result(column="PowerId", property="powerid", jdbcType=JdbcType.INTEGER),
        @Result(column="RoleId", property="roleid", jdbcType=JdbcType.INTEGER)
    })
    List<Powerroleinfo> selectAll();

    @Update({
        "update powerroleinfo",
        "set PowerId = #{powerid,jdbcType=INTEGER},",
          "RoleId = #{roleid,jdbcType=INTEGER}",
        "where Id = #{id,jdbcType=INTEGER}"
    })
    int updateByPrimaryKey(Powerroleinfo record);
}