import React from 'react';

interface ModalProps {
    isOpen: boolean;
    onClose: () => void;
    title: string;
    children: React.ReactNode;
}

export default function Modal({ isOpen, onClose, title, children }: ModalProps) {
    if (!isOpen) return null;

    return (
        <div className="fixed inset-0 flex items-center justify-center z-50">
            <div className="absolute inset-0 bg-black opacity-60" onClick={onClose}></div>

            <div className="bg-white w-11/12 md:max-w-md mx-auto rounded shadow-lg z-50 overflow-y-auto">
                <div className="py-4 text-left px-6">
                    <div className="flex justify-between items-center pb-3">
                        <p className="text-2xl font-bold">{title}</p>
                        <div className="cursor-pointer z-50" onClick={onClose}>
                            <svg className="fill-current text-black" xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 18 18">
                                <path d="M18 1.3L16.7 0 9 7.7 1.3 0 0 1.3 7.7 9 0 16.7 1.3 18 9 10.3l7.7 7.7 1.3-1.3L10.3 9 18 1.3z"/>
                            </svg>
                        </div>
                    </div>

                    {children}
                </div>
            </div>
        </div>
    );
}