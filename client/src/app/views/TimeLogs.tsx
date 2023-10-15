import React, { useState, useEffect } from 'react';
// import Table from "../components/Table";
import {getTimelogsByProjectId,TimeLog} from "../api/timelogs";   
import  TimelogsForm from './AddTimeLog';

interface TimeLogsProps {
    projectId: number | null;
}
export default function TimeLogs({ projectId }: TimeLogsProps){
    const [timelogs, setTimelogs] = useState<TimeLog[]>([]); 
    const [showModal, setShowModal] = useState<boolean>(false);
    const fetchTimeLogs = async () => {
        const logs = await getTimelogsByProjectId(projectId);
        setTimelogs(logs);
    }


    const closeModal = async  ()=>{
        setShowModal(false);
        fetchTimeLogs();
  }

    useEffect(() => {
        if (projectId !== null) {
 
            fetchTimeLogs();
        }
    }, [projectId]);
    
    if (projectId === null) {
        return null; // Or return some placeholder if desired.
    }
    return (
        <div> 

                <div className="w-1/2">
                    <button onClick={() => setShowModal(true)} className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">
                        Add entry
                    </button>
                </div>
                {showModal && (
                    <TimelogsForm projectId={projectId} onClose={() => closeModal()} />
                )} 

            <table className="table-fixed w-full">
            <thead className="bg-gray-200">
                <tr>
                    <th className="border px-4 py-2 w-12">Id</th>
                    <th className="border px-4 py-2">Comment</th>
                    <th className="border px-4 py-2">Log Date</th>
                    <th className="border px-4 py-2">Minutes</th> 
                </tr>
            </thead>
            <tbody>


            {timelogs.map(timelog => (
                 <tr key = {timelog.id}>
                    <td className="border px-4 py-2 w-12">{timelog.id}</td>
                    <td className="border px-4 py-2">{timelog.comment}</td>
                    <td className="border px-4 py-2">{timelog.logDate}</td>                    
                    <td className="border px-4 py-2">{timelog.logTimeInMinutes}</td>         
                 </tr>                    
                ))} 
            </tbody>
        </table>             
        </div>
    );
}
