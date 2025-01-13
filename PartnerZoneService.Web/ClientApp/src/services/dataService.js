import axios from "axios";

async function getZones(){
    return await axios({
        method: "get",
        url: "https://localhost:7253/PartnerZone",
        headers: {}
    });
}

export default getZones();