import React, {Component, useEffect, useState} from 'react';
import {Table} from "reactstrap";
import getZones from "../services/dataService";

function Home(){
    const [zones, setZones] = useState([]);
    
    const fetchZones = async () => {
        let res = await getZones;
        if (res.status === 200){
            setZones(JSON.parse(res.data.message));
            console.log(zones.PartnerZones);
        }
    }
    
    useEffect(() => {
        fetchZones();
    }, []);
    
    return(
        <Table striped bordered hover>
            <thead>
            <tr>
                <th>#</th>
                <th>Id</th>
                <th>Name</th>
                <th>Description</th>
                <th>Status</th>
                <th>TariffId</th>
                <th>IsArchive</th>
            </tr>
            </thead>
            <tbody>
            {zones.PartnerZones && zones.PartnerZones.map((zone, index) => {
                return (
                    <tr>
                        <td></td>
                        <td>{zone.Id}</td>
                        <td>{zone.Name}</td>
                        <td>{zone.Description}</td>
                        <td>{zone.Status}</td>
                        <td>{zone.TariffId}</td>
                        <td>{String(zone.IsArchive)}</td>
                    </tr>
                )
            })}
            </tbody>
        </Table>
    )
}

export default Home;
