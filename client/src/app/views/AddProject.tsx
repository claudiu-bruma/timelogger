import React, { useState } from 'react';
import { createProject, NewProject } from "../api/projects";  

export default function ProjectForm({ onClose }: AddProjectProps) {
    const [name, setName] = useState<string>('');
    const [deadline, setDeadline] = useState<Date | null>(null);
    const [isCompleted, setIsCompleted] = useState<boolean>(false);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (name && deadline) {
            const newProject: NewProject = { name, deadline, isCompleted };
            console.log(newProject);
            const isResponseOk =  await createProject(newProject);
            console.log(isResponseOk);
            // Reset form
            setName('');
            setDeadline(null);
            setIsCompleted(false);
            onClose();
        }
    };

    return (
        <div className="modal flex items-center my-6">
            <form id="addProjectForm"  onSubmit={handleSubmit}>
                <div>
                    <label>
                        Project Name:
                        <input value={name} onChange={e => setName(e.target.value)} />
                    </label>
                </div>
                <div>
                    <label>
                        Deadline:
                     <input type="date" onChange={e => setDeadline(new Date(e.target.value))} />
                    </label>
                </div>
                <div>
                     <label>
                     Completed:
                     <input type="checkbox" checked={isCompleted} onChange={e => setIsCompleted(e.target.checked)} />
                </label> 
                </div>
                <div>       
                     <button className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"  type="submit">Save</button>        
                     <button className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded" onClick={onClose}>Close</button>     
                </div>
            </form>
        </div>
    );
}

interface AddProjectProps {
    onClose: () => void;
}