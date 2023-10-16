import React, { useState, useEffect } from 'react';
import {getTimelogsByProjectId} from "../api/timelogs";   
import { TimeLog } from "../models/TimeLog";
import  TimelogsForm from './AddTimeLog';
import Modal from "../components/Modal";

interface TimeLogsProps {
    projectId: number | null;
    onClose: () => void;
}
export default function TimeLogs({ projectId , onClose}: TimeLogsProps){
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
        return null; 
    }
    return (
        <div>
        <div className="flex items-center my-6">

                <div className="flex space-x-2"> 
                    <button className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" onClick={() => onClose()}>Back</button>
                    <button onClick={() => setShowModal(true)} className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">
                        Add entry
                    </button>
                </div>


                <Modal isOpen={showModal} onClose={() => closeModal()} title="Add Timelog">
                    <TimelogsForm projectId={projectId} onClose={() => closeModal()} />
                </Modal>
 
            </div>  
            <table className="table-fixed w-full">
            <thead className="bg-gray-200">
                <tr> 
                    <th className="border px-4 py-2">Comment</th>
                    <th className="border px-4 py-2">Log Date</th>
                    <th className="border px-4 py-2">Minutes</th> 
                </tr>
            </thead>
            <tbody>


            {timelogs.map(timelog => (
                 <tr key = {timelog.id}> 
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
