import http from "../http-common";

const getAll = () => {
  return http.get("/DBLog");
};
const postData = (filter:number, user:any, message:any) => {
  return http.post("/DBLog?filter="+filter+"&user="+user+"&message="+message);
};

const Service = {
    getAll,
    postData
  };
  
export default Service;